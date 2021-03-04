using System.Collections.Generic;

namespace Cortside.SqlReportApi.Data {

    public class ReportResult {
        public string Name { get; set; }
        public IList<ReportColumn> Columns { get; set; }
        public IList<object[]> Rows { get; set; }

        public ReportResult(string name) {
            this.Name = name;
            this.Columns = new List<ReportColumn>();
            this.Rows = new List<object[]>();
        }
    }
}
