using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Cortside.SqlReportApi.DomainService
{
    public interface ISqlReportService
    {
        Report GetReport(string name);

        IList<Report> GetReports();

        IList<ReportGroup> GetReportGroups();

        ReportGroup GetReportGroup(int id);

        IList<ReportArgument> GetReportArguments();

        ReportArgument GetReportArgument(int id);

        IList<ReportArgumentQuery> GetReportArgumentQueries();

        ReportArgumentQuery GetReportArgumentQuery(int id);

        Task<ReportResult> ExecuteReport(string name, IQueryCollection query, List<string> permissions);

        Stream ExportReport(ReportResult report);
    }
}
