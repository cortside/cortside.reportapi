using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.SqlReportApi.Domain {
    [Table("ReportArgumentQuery")]
    public class ReportArgumentQuery {
        public int ReportArgumentQueryId { get; set; }
        public string ArgQuery { get; set; }
    }
}
