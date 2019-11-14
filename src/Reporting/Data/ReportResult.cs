using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Reporting.Data {
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
