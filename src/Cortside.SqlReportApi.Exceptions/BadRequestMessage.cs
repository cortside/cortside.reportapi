using Cortside.Common.Messages.MessageExceptions;

namespace Cortside.SqlReportApi.Exceptions {
    public class BadRequestMessage : BadRequestResponseException {
        public BadRequestMessage() : base("Invalid arguments") {
        }

        public BadRequestMessage(string message) : base(message) {
        }
    }
}
