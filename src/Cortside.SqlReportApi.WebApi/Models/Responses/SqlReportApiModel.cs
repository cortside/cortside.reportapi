using System;

namespace Cortside.SqlReportApi.WebApi.Models.Responses {
    /// <summary>
    /// Represents a single loan
    /// </summary>
    public class SqlReportApiModel {
        /// <summary>
        /// Unique identifier for a SqlReportApi
        /// </summary>
        public Guid SqlReportApiId { get; set; }

        /// <summary>
        /// SqlReportApi type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Create Subject
        /// </summary>
        public SubjectModel CreatedSubject { get; set; }

        /// <summary>
        /// LastModifiedDate
        /// </summary>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// LastModifiedSubject
        /// </summary>
        public SubjectModel LastModifiedSubject { get; set; }

        /// <summary>
        /// SqlReportApi filename
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// SqlReportApi file hash
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// SqlReportApi file size
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// SqlReportApi set id for application
        /// </summary>
        public long? SqlReportApietId { get; set; }

        /// <summary>
        /// Date SqlReportApi were uploaded
        /// </summary>
        public DateTime? SqlReportApiUploadDate { get; set; }

        /// <summary>
        /// Date contractor printed SqlReportApi
        /// </summary>
        public DateTime? ContractorPrintedDate { get; set; }
    }
}
