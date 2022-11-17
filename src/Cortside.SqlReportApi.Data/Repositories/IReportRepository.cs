using System.Collections.Generic;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Cortside.SqlReportApi.Data.Repositories {
    public interface IReportRepository {
        Task<ReportResult> ExecuteReportAsync(string name, IQueryCollection args, List<string> permissions);
        Task<ReportArgument> GetReportArgumentAsync(int id);
        Task<IList<ReportArgumentQuery>> GetReportArgumentQueriesAsync();
        Task<ReportArgumentQuery> GetReportArgumentQueryAsync(int id);
        Task<IList<ReportArgument>> GetReportArgumentsAsync();
        Task<Report> GetReportAsync(string name);
        Task<ReportGroup> GetReportGroupAsync(int id);
        Task<IList<ReportGroup>> GetReportGroupsAsync();
        Task<IList<Report>> GetReportsAsync();
    }
}