using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Reporting.Entity {
    [Table("ReportArgumentQuery")]
    public class ReportArgumentQuery {
        public int ReportArgumentQueryId { get; set; }
        public string ArgQuery { get; set; }
    }
}
