using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Cortside.SqlReportApi.WebApi.IntegrationTests.Tests {
    public class SqlReportApiTests : IClassFixture<IntegrationTestFactory<Startup>> {
        private readonly IntegrationTestFactory<Startup> fixture;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly HttpClient testServerClient;

        public SqlReportApiTests(IntegrationTestFactory<Startup> fixture, ITestOutputHelper testOutputHelper) {
            this.fixture = fixture;
            this.testOutputHelper = testOutputHelper;
            testServerClient = fixture.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }
    }
}
