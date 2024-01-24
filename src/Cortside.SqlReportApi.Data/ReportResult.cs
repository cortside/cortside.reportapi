using System.Collections.Generic;

namespace Cortside.SqlReportApi.Data {
    public class ReportResult {
        public ReportResult(string name) {
            Name = name;
            ResultSets = new List<ResultSet>();
        }

        public string Name { get; set; }
        public IList<ResultSet> ResultSets { get; set; }
    }
}
