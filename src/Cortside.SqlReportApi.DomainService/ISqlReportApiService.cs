using System.Collections.Generic;
using Cortside.SqlReportApi.Domain;

namespace Cortside.SqlReportApi.DomainService {

    public interface ISqlReportApiService {

        Report GetReport(string name);

        IEnumerable<Report> GetReports();

        IEnumerable<ReportGroup> GetReportGroups();

        ReportGroup GetReportGroup(int id);

        IEnumerable<ReportArgument> GetReportArguments();

        ReportArgument GetReportArgument(int id);

        IEnumerable<ReportArgumentQuery> GetReportArgumentQueries();

        ReportArgumentQuery GetReportArgumentQuery(int id);
    }
}
