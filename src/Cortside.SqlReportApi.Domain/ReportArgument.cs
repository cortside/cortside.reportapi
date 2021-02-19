using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.SqlReportApi.Domain {

    [Table("ReportArgument")]
    public class ReportArgument {
        public int ReportArgumentId { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string ArgName { get; set; }

        [NotMapped]
        public Dictionary<object, object> ArgValues { get; set; }

        public string ArgType { get; set; }
        public int? ReportArgumentQueryId { get; set; }
        public virtual ReportArgumentQuery ReportArgumentQuery { get; set; }
        public int Sequence { get; set; }
    }
}
