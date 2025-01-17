using Cortside.Common.Messages.MessageExceptions;

namespace Cortside.SqlReportApi.Exceptions {
    public class BadRequestMessage : BadRequestResponseException {
        public BadRequestMessage() : base("Invalid arguments") {
        }

        public BadRequestMessage(string message) : base(message) {
        }

        public BadRequestMessage(string message, System.Exception exception) : base(message, exception) {
        }

        protected BadRequestMessage(string key, string property, params object[] properties) : base(key, property, properties) {
        }

        protected BadRequestMessage(string message, string property) : base(message, property) {
        }
    }
}
