using System;

namespace Cortside.SqlReportApi.Exceptions {
    public class NotAuthorizedMessage : Exception {
        public NotAuthorizedMessage() : base($"User does not have the required permission.") { }
        public NotAuthorizedMessage(string message) : base(message) { }
        public NotAuthorizedMessage(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
