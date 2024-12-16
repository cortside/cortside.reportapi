using System;
using System.Runtime.Serialization;
using Cortside.Common.Messages.MessageExceptions;

namespace Cortside.SqlReportApi.Exceptions {
    [Serializable]
    public class ExternalCommunicationFailureMessage : InternalServerErrorResponseException {
        public ExternalCommunicationFailureMessage(string message) : base(message) {
        }

        public ExternalCommunicationFailureMessage() : base("error communicating with an external service.") {
        }

        protected ExternalCommunicationFailureMessage(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        public ExternalCommunicationFailureMessage(string message, Exception exception) : base(message, exception) {
        }

        protected ExternalCommunicationFailureMessage(string key, string property, params object[] properties) : base(key, property, properties) {
        }

        protected ExternalCommunicationFailureMessage(string message, string property) : base(message, property) {
        }
    }
}
