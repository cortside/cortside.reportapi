using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using Cortside.Common.BootStrap;
using Cortside.Common.Correlation;
using Cortside.Common.Json;
using Cortside.Health.Controllers;
using Cortside.SqlReportApi.BootStrap;
using Cortside.SqlReportApi.DomainService;
using IdentityServer4.AccessTokenValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;


namespace Cortside.SqlReportApi.WebApi {
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup {
        private readonly BootStrapper bootstrapper = null;

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration) {
            bootstrapper = new DefaultApplicationBootStrapper();
            Configuration = configuration;
        }

        /// <summary>
        /// Config
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure Services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services) {

            services.AddResponseCaching();
            services.AddResponseCompression(options => {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddCors();

            IsoDateTimeConverter isoConverter = new IsoDateTimeConverter {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"
            };

            IsoTimeSpanConverter isoTimeSpanConverter = new IsoTimeSpanConverter();

            JsonConvert.DefaultSettings = (() => {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                settings.Converters.Add(isoConverter);
                settings.Converters.Add(isoTimeSpanConverter);
                return settings;
            });

            services.AddControllers(options => {
                options.CacheProfiles.Add("default", new CacheProfile {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any
                });
                //options.Filters.Add<MessageExceptionResponseFilter>();
            })
            .AddNewtonsoftJson(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                options.SerializerSettings.Converters.Add(isoConverter);
                options.SerializerSettings.Converters.Add(isoTimeSpanConverter);
            })
            .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(HealthController).Assembly));

            services.AddRouting(options => {
                options.LowercaseUrls = true;
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(sp => {
                return sp.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(sp.GetRequiredService<IActionContextAccessor>().ActionContext);
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var authConfig = Configuration.GetSection("IdentityServer");

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options => {
                    // base-address of your identityserver
                    options.Authority = authConfig.GetValue<string>("Authority");
                    options.RequireHttpsMetadata = authConfig.GetValue<bool>("RequireHttpsMetadata");
                    options.RoleClaimType = "role";
                    options.NameClaimType = "name";

                    // name of the API resource
                    options.ApiName = authConfig.GetValue<string>("ApiName");
                    options.ApiSecret = authConfig.GetValue<string>("ApiSecret");

                    options.EnableCaching = authConfig.GetValue<bool>("CacheEnabled");
                    options.CacheDuration = authConfig.GetValue<TimeSpan>("CacheDuration");
                });

            services.AddOptions();
            services.AddHttpContextAccessor();

            services.AddTransient<ISqlReportService, SqlReportService>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "SqlReport API" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            string instrumentationKey = Configuration["ApplicationInsights:InstrumentationKey"];
            services.AddSingleton<ITelemetryInitializer, AppInsightsInitializer>();
            services.AddApplicationInsightsTelemetry(o => {
                o.InstrumentationKey = instrumentationKey;
                o.EnableAdaptiveSampling = false;
                o.EnableActiveTelemetryConfigurationSetup = true;
            });

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddPolicyServerRuntimeClient(Configuration.GetSection("PolicyServer"))
                .AddAuthorizationPermissionPolicies();
            services.AddSingleton<ISubjectService, SubjectService>();

            services.AddSingleton(Configuration);
            bootstrapper.InitIoCContainer(Configuration as IConfigurationRoot, services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseMiddleware<CorrelationMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                var s = "/swagger/v1/swagger.json";
                c.SwaggerEndpoint(s, "SqlReport Api V1");
            });

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseCors(builder => builder
                .WithOrigins(Configuration.GetSection("Cors").GetSection("Origins").Get<string[]>())
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints => {
                    endpoints.MapControllers();
                });
        }
    }
}
