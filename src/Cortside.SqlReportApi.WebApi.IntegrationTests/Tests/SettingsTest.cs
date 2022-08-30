using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Cortside.SqlReportApi.WebApi.IntegrationTests.Tests {
    public class SettingsTest : IClassFixture<IntegrationTestFactory<Startup>> {
        private readonly HttpClient testServerClient;

        public SettingsTest(IntegrationTestFactory<Startup> fixture) {
            testServerClient = fixture.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task TestAsync() {
            //arrange

            //act
            var response = await testServerClient.GetAsync("api/settings").ConfigureAwait(false);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
