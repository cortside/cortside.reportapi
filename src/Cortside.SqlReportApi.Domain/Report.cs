using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Cortside.SqlReportApi.Domain {
    [Table("Report")]
    public class Report {
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReportGroupId { get; set; }
        public virtual ReportGroup ReportGroup { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual ICollection<ReportArgument> ReportArguments { get; set; }

        public Report() {
            this.ReportArguments = new HashSet<ReportArgument>();
        }
    }
}
