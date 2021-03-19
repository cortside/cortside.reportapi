using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.SqlReportApi.Domain {
    [Table("ReportGroup")]
    public class ReportGroup {
        public int ReportGroupId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}
