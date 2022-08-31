using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cortside.AspNetCore.Common.Models;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.DomainService;
using Cortside.SqlReportApi.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using PolicyServer.Runtime.Client;
using Xunit;

namespace Cortside.SqlReportApi.WebApi.Tests {

    public class SqlReportApiControllerTest : ControllerTest<ReportArgumentController> {
        private Mock<ISqlReportService> serviceMock;
        private Mock<IPolicyServerRuntimeClient> policyClientMock;
        private ReportArgumentController reportArgumentController;
        private ReportArgumentQueryController reportArgumentQueryController;
        private ReportController reportController;
        private ReportGroupController reportGroupController;
        private const string permission = "permission";

        public SqlReportApiControllerTest() : base() {
            serviceMock = testFixture.Mock<ISqlReportService>();
            policyClientMock = new Mock<IPolicyServerRuntimeClient>();
            reportArgumentController = new ReportArgumentController(serviceMock.Object);
            reportArgumentQueryController = new ReportArgumentQueryController(serviceMock.Object);
            reportController = new ReportController(serviceMock.Object, policyClientMock.Object);
            reportGroupController = new ReportGroupController(serviceMock.Object);

            var configurationSection = new Mock<IConfigurationSection>();
            var configuration = new Mock<IConfiguration>();

            policyClientMock.Setup(p => p.EvaluateAsync(It.IsAny<PolicyEvaluationRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new PolicyResult("", new List<string>(), new List<string>() { permission })));
            configurationSection.Setup(a => a.GetSection(It.Is<string>(s => s == "BasePolicyPrefix")).Value).Returns("Sql Report");
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "PolicyServer")))
                .Returns(configurationSection.Object);
        }

        [Fact]
        public async Task GetReportArgumentShouldReturnObjectAsync() {
            //arrange
            var reportArgument = new ReportArgument() {
                ReportArgumentId = 1,
                ArgName = "argName",
                ArgType = "string"
            };
            serviceMock.Setup(s => s.GetReportArgumentAsync(It.IsAny<int>())).ReturnsAsync(reportArgument);

            //act
            var result = await reportArgumentController.GetAsync(1).ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgument);
            serviceMock.Verify(s => s.GetReportArgumentAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetReportArgumentsShouldReturnObjectAsync() {
            //arrange
            var reportArgument = new List<ReportArgument>() {
                new ReportArgument() {
                    ReportArgumentId = 1,
                    ArgName = "argName",
                    ArgType = "string"
                }
            };
            serviceMock.Setup(s => s.GetReportArgumentsAsync()).ReturnsAsync(reportArgument);

            //act
            var result = await reportArgumentController.GetAsync().ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgument);
            serviceMock.Verify(s => s.GetReportArgumentsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetReportArgumentQueryShouldReturnObjectAsync() {
            //arrange
            var reportArgumentQuery = new ReportArgumentQuery() {
                ArgQuery = "someQuery",
                ReportArgumentQueryId = 1
            };
            serviceMock.Setup(s => s.GetReportArgumentQueryAsync(It.IsAny<int>())).ReturnsAsync(reportArgumentQuery);

            //act
            var result = await reportArgumentQueryController.GetAsync(1).ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgumentQuery);
            serviceMock.Verify(s => s.GetReportArgumentQueryAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetReportArgumentQueriesShouldReturnObjectAsync() {
            //arrange
            var reportArgumentQuery = new List<ReportArgumentQuery>() {
                new ReportArgumentQuery() {
                    ArgQuery = "someQuery",
                    ReportArgumentQueryId = 1
                }
            };
            serviceMock.Setup(s => s.GetReportArgumentQueriesAsync()).ReturnsAsync(reportArgumentQuery);

            //act
            var result = await reportArgumentQueryController.GetAsync().ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgumentQuery);
            serviceMock.Verify(s => s.GetReportArgumentQueriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetReportShouldReturnReportAsync() {
            //arrange
            var request = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();

            var reportRequest = new Report() {
                Name = "someName",
                Description = "someDescription",
                Permission = permission
            };
            var report = new ReportResult(reportRequest.Name);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.Add("date", new StringValues("01/01/2000"));
            var collection = new QueryCollection(dictionary);

            request.SetupGet(x => x.Query).Returns(collection);
            context.SetupGet(x => x.Request).Returns(request.Object);
            serviceMock.Setup(s => s.ExecuteReportAsync(reportRequest.Name, collection, It.IsAny<List<string>>())).Returns(Task.FromResult(report));

            reportController.ControllerContext = new ControllerContext(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));

            //act
            var result = await reportController.GetAsync(reportRequest.Name).ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(report);
            serviceMock.Verify(s => s.ExecuteReportAsync(reportRequest.Name, collection, It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public async Task GetReportsShouldReturnObjectAsync() {
            //arrange
            var report = new List<Report>() {
                new Report() {
                    Name = "someName",
                    Description = "someDescription"
                }
            };
            serviceMock.Setup(s => s.GetReportsAsync()).ReturnsAsync(report);

            //act
            var result = await reportController.GetAsync().ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(report);
            serviceMock.Verify(s => s.GetReportsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetReportGroupShouldReturnObjectAsync() {
            //arrange
            var reportGroup = new ReportGroup() {
                Name = "someGroup",
                ReportGroupId = 1
            };
            serviceMock.Setup(s => s.GetReportGroupAsync(It.IsAny<int>())).ReturnsAsync(reportGroup);

            //act
            var result = await reportGroupController.GetAsync(1).ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportGroup);
            serviceMock.Verify(s => s.GetReportGroupAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetReportGroupsShouldReturnObjectAsync() {
            //arrange
            var reportGroup = new List<ReportGroup>() {
                new ReportGroup() {
                    Name = "someGroup",
                    ReportGroupId = 1
                }
            };
            serviceMock.Setup(s => s.GetReportGroupsAsync()).ReturnsAsync(reportGroup);

            //act
            var result = await reportGroupController.GetAsync().ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<OkObjectResult>();
            var content = viewResult.Which.Value as ListResult<ReportGroup>;
            content.Results.Should().BeEquivalentTo(reportGroup);
            serviceMock.Verify(s => s.GetReportGroupsAsync(), Times.Once);
        }


        [Fact]
        public async Task ExportReportShouldReturnObjectAsync() {
            //arrange
            var request = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();
            var dictionary = new Dictionary<string, StringValues>();
            dictionary.Add("date", new StringValues("01/01/2000"));
            var collection = new QueryCollection(dictionary);

            request.SetupGet(x => x.Query).Returns(collection);
            context.SetupGet(x => x.Request).Returns(request.Object);


            var report = new ReportResult("my report") {
                ResultSets = { new ResultSet {
                Columns = new List<ReportColumn> {
                    new ReportColumn {
                        Name = "column1"
                    },
                    new ReportColumn {
                        Name = "column2"
                    }
                },
                Rows = new List<object[]> {
                    new object[] {
                        "row1column1",
                        "row1column2"},
                    new object[] {
                        "row2column1",
                        "row2column2"
                    },
                }
                }
                }
            };

            using MemoryStream stream = new MemoryStream();
            using StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("column1", "column2");
            writer.WriteLine("row1column1", "row1column2");
            writer.WriteLine("row2column1", "row2column2");

            reportController.ControllerContext = new ControllerContext(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));
            serviceMock.Setup(s => s.ExecuteReportAsync(It.IsAny<string>(), It.IsAny<QueryCollection>(), It.IsAny<List<string>>())).Returns(Task.FromResult(report));
            serviceMock.Setup(s => s.ExportReport(It.IsAny<ReportResult>())).Returns(stream);

            //act
            var result = await reportController.ExportAsync("report").ConfigureAwait(false);

            //assert
            var viewResult = result.Should().BeAssignableTo<FileStreamResult>();
        }
    }
}
