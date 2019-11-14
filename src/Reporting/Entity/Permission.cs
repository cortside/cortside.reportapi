using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Reporting.Entity {
    [Table("Permission")]
    public class Permission {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
