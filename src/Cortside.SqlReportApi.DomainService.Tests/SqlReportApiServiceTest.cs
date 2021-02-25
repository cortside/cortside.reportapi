using System.Linq;
using System.Threading.Tasks;
using Cortside.Common.DomainEvent;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Cortside.SqlReportApi.DomainService.Tests {

    public class SqlReportApierviceTest : DomainServiceTest<ISqlReportService> {
        private readonly IDatabaseContext databaseContext;
        private readonly Mock<IDomainEventPublisher> domainEventPublisherMock;
        private readonly Mock<ILogger<SqlReportService>> loggerMock;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly ISqlReportService service;
        private Report report;
        private ReportGroup group;
        private ReportArgument reportArgument;
        private ReportArgumentQuery argumentQuery;
        private Permission permission;

        public SqlReportApierviceTest(ITestOutputHelper testOutputHelper) : base() {
            databaseContext = GetDatabaseContext();
            domainEventPublisherMock = testFixture.Mock<IDomainEventPublisher>();
            loggerMock = testFixture.Mock<ILogger<SqlReportService>>();
            this.testOutputHelper = testOutputHelper;

            service = new SqlReportService(databaseContext, loggerMock.Object);

            permission = new Permission() { PermissionId = 1, Name = "someName", Description = "someDescription" };
            argumentQuery = new ReportArgumentQuery() { ArgQuery = "query", ReportArgumentQueryId = 1 };
            reportArgument = new ReportArgument { Name = "argumentName", ReportArgumentId = 1 };
            group = new ReportGroup() { Name = "groupName", ReportGroupId = 1 };
            report = new Report() {
                Description = "someDescription",
                Name = "someName",
                Permission = permission,
                PermissionId = permission.PermissionId,
                ReportId = reportArgument.ReportId,
                ReportArguments = null,
                ReportGroup = null,
                ReportGroupId = group.ReportGroupId
            };
        }

        [Fact]
        public async Task ShouldGetReport() {
            //arrange
            databaseContext.Reports.Add(report);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReport(report.Name);

            //assert
            result.Should().BeEquivalentTo(report);
        }

        [Fact]
        public async Task ShouldGetReports() {
            //arrange
            databaseContext.Reports.Add(report);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReports();

            //assert
            result.FirstOrDefault().Should().BeEquivalentTo(report);
        }

        [Fact]
        public async Task ShouldGetReportGroup() {
            //arrange
            databaseContext.ReportGroups.Add(group);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReportGroup(group.ReportGroupId);

            //assert
            result.Should().BeEquivalentTo(group);
        }

        [Fact]
        public async Task ShouldGetReportGroups() {
            //arrange
            databaseContext.ReportGroups.Add(group);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReportGroups();

            //assert
            result.FirstOrDefault().Should().BeEquivalentTo(group);
        }

        [Fact]
        public async Task ShouldGetReportArgument() {
            //arrange
            databaseContext.ReportArguments.Add(reportArgument);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReportArgument(reportArgument.ReportArgumentId);

            //assert
            result.Should().BeEquivalentTo(reportArgument);
        }

        [Fact]
        public async Task ShouldGetReportArguments() {
            //arrange
            databaseContext.ReportArguments.Add(reportArgument);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReportArguments();

            //assert
            result.FirstOrDefault().Should().BeEquivalentTo(reportArgument);
        }

        [Fact]
        public async Task ShouldGetReportArgumentQuery() {
            //arrange
            databaseContext.ReportArgumentQuerys.Add(argumentQuery);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReportArgumentQuery(argumentQuery.ReportArgumentQueryId);

            //assert
            result.Should().BeEquivalentTo(argumentQuery);
        }

        [Fact]
        public async Task ShouldGetReportArgumentsQueries() {
            //arrange
            databaseContext.ReportArgumentQuerys.Add(argumentQuery);

            await databaseContext.SaveChangesAsync();

            //act
            var result = service.GetReportArgumentQueries();

            //assert
            result.FirstOrDefault().Should().BeEquivalentTo(argumentQuery);
        }
    }
}
