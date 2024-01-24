using System;

namespace Cortside.SqlReportApi.WebApi.Models {
    public class AuditableEntityModel {
        public DateTime CreatedDate { get; set; }

        public SubjectModel CreatedSubject { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public SubjectModel LastModifiedSubject { get; set; }
    }
}
