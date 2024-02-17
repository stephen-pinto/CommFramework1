using CommServices.CommShared;

namespace SignalRPeerService.Types
{
    //public enum MessageTypeSigr
    //{
    //    NONE,
    //    REQUEST,
    //    RESPONSE,
    //    NOTIFICATION,
    //    ERROR
    //}

    public record MessageSigr(
            string To,
            string From,
            string Id,
            string Type,
            string Data,
            Dictionary<string, string> Metadata,
            Dictionary<string, string> Headers)
    {
        public static implicit operator MessageSigr(Message protoMessage)
        {
            return new MessageSigr(
                protoMessage.To,
                protoMessage.From,
                protoMessage.Id,
                protoMessage.Type.ToString(),
                protoMessage.Data,
                protoMessage.Metadata.Any() ? protoMessage.Metadata.ToDictionary() : new Dictionary<string, string>(),
                protoMessage.Headers.Any() ? protoMessage.Headers.ToDictionary() : new Dictionary<string, string>());
        }

        public static implicit operator Message(MessageSigr message)
        {
            var protoMessage = new Message();
            protoMessage.To = message.To;
            protoMessage.From = message.From;
            protoMessage.Id = message.Id;
            protoMessage.Type = Enum.Parse<MessageType>(message.Type);
            protoMessage.Data = message.Data;
            protoMessage.Metadata.MergeFrom(message.Metadata);
            protoMessage.Headers.MergeFrom(message.Headers);
            return protoMessage;
        }
    }
}
