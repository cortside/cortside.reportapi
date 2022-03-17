using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.SqlReportApi.Domain {
    [Table("Report")]
    public class Report {
        public int ReportId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public int ReportGroupId { get; set; }
        public virtual ReportGroup ReportGroup { get; set; }
        [StringLength(50)]
        public string Permission { get; set; }
        public virtual ICollection<ReportArgument> ReportArguments { get; set; }

        public Report() {
            this.ReportArguments = new HashSet<ReportArgument>();
        }
    }
}
