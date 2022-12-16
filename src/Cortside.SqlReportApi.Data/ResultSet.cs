using System.Collections.Generic;

namespace Cortside.SqlReportApi.Data {
    public class ResultSet {
        public ResultSet() {
            this.Columns = new List<ReportColumn>();
            this.Rows = new List<object[]>();
        }

        public IList<ReportColumn> Columns { get; set; }
        public IList<object[]> Rows { get; set; }
    }
}
