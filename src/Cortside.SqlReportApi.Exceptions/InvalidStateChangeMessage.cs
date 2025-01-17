using Cortside.Common.Messages.MessageExceptions;

namespace Cortside.SqlReportApi.Exceptions {
    public class InvalidStateChangeMessage : UnprocessableEntityResponseException {
        public InvalidStateChangeMessage() : base("Current state does not allow requested operation.") {
        }

        public InvalidStateChangeMessage(string message) : base($"Current state does not allow requested operation. {message}") {
        }

        public InvalidStateChangeMessage(string message, System.Exception exception) : base(message, exception) {
        }
    }
}
