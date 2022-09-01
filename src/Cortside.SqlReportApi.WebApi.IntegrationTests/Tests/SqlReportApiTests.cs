using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Cortside.SqlReportApi.WebApi.IntegrationTests.Tests {
    public class SqlReportApiTests : IClassFixture<IntegrationTestFactory<Startup>> {
        private readonly HttpClient testServerClient;

        public SqlReportApiTests(IntegrationTestFactory<Startup> fixture) {
            testServerClient = fixture.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }
    }
}
