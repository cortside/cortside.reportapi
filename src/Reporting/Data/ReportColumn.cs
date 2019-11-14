using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Reporting.Data {
    public class ReportColumn {
        public string Name { get; set; }
        public string DataType { get; set; }
        public int Ordinal { get; set; }
    }
}
