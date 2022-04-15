using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain;
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
            reportArgumentController = new ReportArgumentController(testFixture.GetDatabaseContext(), serviceMock.Object, policyClientMock.Object);
            reportArgumentQueryController = new ReportArgumentQueryController(testFixture.GetDatabaseContext(), serviceMock.Object, policyClientMock.Object);
            reportController = new ReportController(testFixture.GetDatabaseContext(), serviceMock.Object, policyClientMock.Object);
            reportGroupController = new ReportGroupController(testFixture.GetDatabaseContext(), serviceMock.Object, policyClientMock.Object);

            var configurationSection = new Mock<IConfigurationSection>();
            var configuration = new Mock<IConfiguration>();

            policyClientMock.Setup(p => p.EvaluateAsync(It.IsAny<PolicyEvaluationRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new PolicyResult("", new List<string>(), new List<string>() { permission })));
            configurationSection.Setup(a => a.GetSection(It.Is<string>(s => s == "BasePolicyPrefix")).Value).Returns("Sql Report");
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "PolicyServer")))
                .Returns(configurationSection.Object);
        }

        [Fact]
        public void GetReportArgumentShouldReturnObject() {
            //arrange
            var reportArgument = new ReportArgument() {
                ReportArgumentId = 1,
                ArgName = "argName",
                ArgType = "string"
            };
            serviceMock.Setup(s => s.GetReportArgument(It.IsAny<int>())).Returns(reportArgument);

            //act
            var result = reportArgumentController.Get(1);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgument);
            serviceMock.Verify(s => s.GetReportArgument(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetReportArgumentsShouldReturnObject() {
            //arrange
            var reportArgument = new List<ReportArgument>() {
                new ReportArgument() {
                    ReportArgumentId = 1,
                    ArgName = "argName",
                    ArgType = "string"
                }
            };
            serviceMock.Setup(s => s.GetReportArguments()).Returns(reportArgument);

            //act
            var result = reportArgumentController.Get();

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgument);
            serviceMock.Verify(s => s.GetReportArguments(), Times.Once);
        }

        [Fact]
        public void GetReportArgumentQueryShouldReturnObject() {
            //arrange
            var reportArgumentQuery = new ReportArgumentQuery() {
                ArgQuery = "someQuery",
                ReportArgumentQueryId = 1
            };
            serviceMock.Setup(s => s.GetReportArgumentQuery(It.IsAny<int>())).Returns(reportArgumentQuery);

            //act
            var result = reportArgumentQueryController.Get(1);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgumentQuery);
            serviceMock.Verify(s => s.GetReportArgumentQuery(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetReportArgumentQueriesShouldReturnObject() {
            //arrange
            var reportArgumentQuery = new List<ReportArgumentQuery>() {
                new ReportArgumentQuery() {
                    ArgQuery = "someQuery",
                    ReportArgumentQueryId = 1
                }
            };
            serviceMock.Setup(s => s.GetReportArgumentQueries()).Returns(reportArgumentQuery);

            //act
            var result = await reportArgumentQueryController.Get();

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportArgumentQuery);
            serviceMock.Verify(s => s.GetReportArgumentQueries(), Times.Once);
        }

        [Fact]
        public async Task GetReportShouldReturnReport() {
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
            serviceMock.Setup(s => s.ExecuteReport(reportRequest.Name, collection, It.IsAny<List<string>>())).Returns(Task.FromResult(report));

            reportController.ControllerContext = new ControllerContext(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));

            //act
            var result = await reportController.Get(reportRequest.Name);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(report);
            serviceMock.Verify(s => s.ExecuteReport(reportRequest.Name, collection, It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public async Task GetReportsShouldReturnObject() {
            //arrange
            var report = new List<Report>() {
                new Report() {
                    Name = "someName",
                    Description = "someDescription"
                }
            };
            serviceMock.Setup(s => s.GetReports()).Returns(report);

            //act
            var result = await reportController.Get();

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(report);
            serviceMock.Verify(s => s.GetReports(), Times.Once);
        }

        [Fact]
        public void GetReportGroupShouldReturnObject() {
            //arrange
            var reportGroup = new ReportGroup() {
                Name = "someGroup",
                ReportGroupId = 1
            };
            serviceMock.Setup(s => s.GetReportGroup(It.IsAny<int>())).Returns(reportGroup);

            //act
            var result = reportGroupController.Get(1);

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportGroup);
            serviceMock.Verify(s => s.GetReportGroup(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetReportGroupsShouldReturnObject() {
            //arrange
            var reportGroup = new List<ReportGroup>() {
                new ReportGroup() {
                    Name = "someGroup",
                    ReportGroupId = 1
                }
            };
            serviceMock.Setup(s => s.GetReportGroups()).Returns(reportGroup);

            //act
            var result = reportGroupController.Get();

            //assert
            var viewResult = result.Should().BeAssignableTo<ObjectResult>();
            viewResult.Which.Value.Should().BeEquivalentTo(reportGroup);
            serviceMock.Verify(s => s.GetReportGroups(), Times.Once);
        }


        [Fact]
        public async void ExportReportShouldReturnObject() {
            //arrange
            var request = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();
            var dictionary = new Dictionary<string, StringValues>();
            dictionary.Add("date", new StringValues("01/01/2000"));
            var collection = new QueryCollection(dictionary);

            request.SetupGet(x => x.Query).Returns(collection);
            context.SetupGet(x => x.Request).Returns(request.Object);


            var report = new ReportResult("my report") {
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
            };

            using MemoryStream stream = new MemoryStream();
            using StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("column1", "column2");
            writer.WriteLine("row1column1", "row1column2");
            writer.WriteLine("row2column1", "row2column2");

            reportController.ControllerContext = new ControllerContext(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));
            serviceMock.Setup(s => s.ExecuteReport(It.IsAny<string>(), It.IsAny<QueryCollection>(), It.IsAny<List<string>>())).Returns(Task.FromResult(report));
            serviceMock.Setup(s => s.ExportReport(It.IsAny<ReportResult>())).Returns(stream);

            //act
            var result = await reportController.Export("report");

            //assert
            var viewResult = result.Should().BeAssignableTo<FileStreamResult>();
        }
    }
}
