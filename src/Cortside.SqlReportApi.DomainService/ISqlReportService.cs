using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Cortside.SqlReportApi.DomainService {
    public interface ISqlReportService {
        Task<Report> GetReportAsync(string name);

        Task<IList<Report>> GetReportsAsync();

        Task<IList<ReportGroup>> GetReportGroupsAsync();

        Task<ReportGroup> GetReportGroupAsync(int id);

        Task<IList<ReportArgument>> GetReportArgumentsAsync();

        Task<ReportArgument> GetReportArgumentAsync(int id);

        Task<IList<ReportArgumentQuery>> GetReportArgumentQueriesAsync();

        Task<ReportArgumentQuery> GetReportArgumentQueryAsync(int id);

        Task<ReportResult> ExecuteReportAsync(string name, IQueryCollection args, List<string> permissions);

        Stream ExportReport(ReportResult report);
    }
}
