using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.SqlReportApi.Domain {

    [Table("Permission")]
    public class Permission {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
