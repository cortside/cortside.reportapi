using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Cortside.Common.Json;
using Cortside.MockServer;
using Cortside.MockServer.AccessControl;
using Cortside.MockServer.AccessControl.Models;
using Cortside.SqlReportApi.Data;
using Medallion.Threading;
using Medallion.Threading.FileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Cortside.SqlReportApi.WebApi.IntegrationTests {
    public class IntegrationTestFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {
        // dbname is created outside of the options so that it's constant and not reevaluated at instance creation time
        private readonly string dbName = Guid.NewGuid().ToString();

        private Subjects subjects;
        public MockHttpServer MockServer { get; private set; }
        private IConfiguration Configuration { get; set; }

        public IntegrationTestFactory() {
            JsonConvert.DefaultSettings = () => {
                var settings = new JsonSerializerSettings();

                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                settings.NullValueHandling = NullValueHandling.Include;
                settings.DefaultValueHandling = DefaultValueHandling.Include;

                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.DateParseHandling = DateParseHandling.DateTimeOffset;
                settings.Converters.Add(new IsoTimeSpanConverter());

                //settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                //settings.Converters.Add(new IsoDateTimeConverter {
                //    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"
                //});
                //settings.Converters.Add(new IsoTimeSpanConverter());
                return settings;
            };
        }

        protected override IHostBuilder CreateHostBuilder() {
            SetupConfiguration();
            SetupLogger();

            MockServer = new MockHttpServer(dbName)
                .ConfigureBuilder<CommonMock>()
                .ConfigureBuilder(new IdentityServerMock("./Data/discovery.json", "./Data/jwks.json"))
                .ConfigureBuilder(new SubjectMock("./Data/subjects.json"));

            var section = Configuration.GetSection("HealthCheckHostedService");
            section["Checks:1:Value"] = $"{MockServer.Url}/api/health";
            section["Checks:2:Value"] = $"{MockServer.Url}/api/health";

            var authConfig = Configuration.GetSection("IdentityServer");
            authConfig["Authority"] = MockServer.Url;
            authConfig["BaseUrl"] = $"{MockServer.Url}/connect/token";
            authConfig["RequireHttpsMetadata"] = "false";

            var policyServerConfig = Configuration.GetSection("PolicyServer");
            var policyserverTokenClient = policyServerConfig.GetSection("TokenClient");
            policyserverTokenClient["Authority"] = MockServer.Url;
            policyServerConfig["PolicyServerUrl"] = MockServer.Url;

            var distributedLockConfig = Configuration.GetSection("DistributedLock");
            distributedLockConfig["UseRedisLockProvider"] = "false";

            MockServer.WaitForStart();

            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder => builder.AddConfiguration(Configuration))
                .ConfigureWebHostDefaults(webbuilder => {
                    webbuilder
                    .UseConfiguration(Configuration)
                    .UseStartup<TStartup>()
                    .ConfigureTestServices(sc => {
                        var useInMemory = bool.Parse(Configuration["IntegrationTestFactory:InMemoryDatabase"]);
                        if (useInMemory) {
                            RegisterDbContext(sc);
                            RegisterFileSystemDistributedLock(sc);
                        }
                    });
                })
                .UseSerilog(Log.Logger);

            return host;
        }

        private void SetupLogger() {
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext();

            var serverUrl = Configuration["Seq:ServerUrl"];
            if (!string.IsNullOrWhiteSpace(serverUrl)) {
                loggerConfiguration.WriteTo.Seq(serverUrl);
            }

            var logFile = Configuration["LogFile:Path"];
            if (!string.IsNullOrWhiteSpace(logFile)) {
                loggerConfiguration.WriteTo.File(logFile);
            }
            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private void SetupConfiguration() {
            Configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.integration.json", optional: false, reloadOnChange: true)
                 .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                 .Build();
        }

        private void RegisterDbContext(IServiceCollection services) {
            // Remove the app's DbContext registration.
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));
            if (descriptor != null) {
                services.Remove(descriptor);
            }
            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContext));
            if (descriptor != null) {
                services.Remove(descriptor);
            }

            // register test instance
            services.AddDbContext<DatabaseContext>(options => {
                options.UseInMemoryDatabase(dbName);
                // enable sensisitive logging for debug logging, NEVER do this in production
                options.EnableSensitiveDataLogging();
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            services.AddScoped<DbContext>(provider => provider.GetService<DatabaseContext>());

            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context (DbContext).
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DatabaseContext>();
            var logger = scopedServices.GetRequiredService<ILogger<IntegrationTestFactory<TStartup>>>();

            // Ensure the database is created.
            db.Database.EnsureCreated();

            try {
                DatabaseFixture.SeedInMemoryDb(db);
                if (!db.Subjects.Any()) {
                    throw new DbUpdateException();
                }
            } catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
            }
        }

        private void RegisterFileSystemDistributedLock(IServiceCollection services) {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDistributedLockProvider));
            if (descriptor != null) {
                services.Remove(descriptor);
            }

            // install a file system one
            services.AddSingleton<IDistributedLockProvider>(_ => {
                return new FileDistributedSynchronizationProvider(new DirectoryInfo(Environment.CurrentDirectory));
            });
        }

        public HttpClient UnauthorizedClient {
            get {
                _UnauthorizedClient ??= CreateDefaultClient();
                return _UnauthorizedClient;
            }
        }

        private HttpClient _UnauthorizedClient { get; set; }

        public HttpClient CreateAuthorizedClient(string clientId) {
            var client = CreateDefaultClient();
            subjects ??= JsonConvert.DeserializeObject<Subjects>(File.ReadAllText("./Data/subjects.json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", subjects.SubjectsList.First(s => s.ClientId == clientId).ReferenceToken);
            return client;
        }

        public HttpClient CreateCustomAuthorizedClient(Uri uri, string clientId) {
            var client = CreateDefaultClient(uri);
            subjects ??= JsonConvert.DeserializeObject<Subjects>(File.ReadAllText("./Data/subjects.json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", subjects.SubjectsList.First(s => s.ClientId == clientId).ReferenceToken);
            return client;
        }

        public Subjects GetAllSubjects() {
            return subjects;
        }

        public Subject GetSubjectByName(string name) {
            return subjects.SubjectsList.First(x => x.ClientId == name);
        }

        internal DatabaseContext NewScopedDbContext() {
            var scope = Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }
    }
}
