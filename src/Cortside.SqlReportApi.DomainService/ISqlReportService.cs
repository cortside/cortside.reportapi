using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain;
using Microsoft.AspNetCore.Http;

namespace Cortside.SqlReportApi.DomainService {

    public interface ISqlReportService {

        Report GetReport(string name);

        IEnumerable<Report> GetReports();

        IEnumerable<ReportGroup> GetReportGroups();

        ReportGroup GetReportGroup(int id);

        IEnumerable<ReportArgument> GetReportArguments();

        ReportArgument GetReportArgument(int id);

        IEnumerable<ReportArgumentQuery> GetReportArgumentQueries();

        ReportArgumentQuery GetReportArgumentQuery(int id);

        Task<ReportResult> ExecuteReport(string name, IQueryCollection query, List<string> permissions);

        Task<StreamContent> ExportReport(string name, IQueryCollection query, List<string> list);
    }
}
