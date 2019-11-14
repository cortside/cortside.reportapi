using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Reporting.Entity {
    [Table("ReportGroup")]
    public class ReportGroup {
        public int ReportGroupId { get; set; }
        public string Name { get; set; }
    }
}
