using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.SqlReportApi.Domain.Entities {
    [Table("ReportArgument")]
    public class ReportArgument {
        public int ReportArgumentId { get; set; }
        public int ReportId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ArgName { get; set; }

        [NotMapped]
        public Dictionary<string, object> ArgValues { get; set; }

        [StringLength(50)]
        public string ArgType { get; set; }

        public int? ReportArgumentQueryId { get; set; }
        public virtual ReportArgumentQuery ReportArgumentQuery { get; set; }
        public int Sequence { get; set; }
    }
}
