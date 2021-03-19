using System;

namespace Cortside.SqlReportApi.Exceptions {
    public class ResourceNotFoundMessage : Exception {
        public ResourceNotFoundMessage() : base("Resource could not be found.") { }
        public ResourceNotFoundMessage(string message) : base(message) { }
        public ResourceNotFoundMessage(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
