using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.SqlReportApi.Domain {
    [Table("ReportGroup")]
    public class ReportGroup {
        public int ReportGroupId { get; set; }
        public string Name { get; set; }
    }
}
