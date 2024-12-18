using EasyRpc.Types;

namespace EasyRpc.Plugin.SignalR.Types
{
    public enum MessageTypeSigr
    {
        None = 0,
        Request = 1,
        Response = 2,
        Notification = 3,
        Error = 4,
        Telemetry = 5,
    }

    public record MessageSigr(
            string To,
            string From,
            string Id,
            MessageTypeSigr Type,
            string Data)
    {
        public Dictionary<string, string> Metadata { get; set; } = new();

        public Dictionary<string, string> Headers { get; set; } = new();

        public static implicit operator MessageSigr(Message protoMessage)
        {
            var instance = new MessageSigr(
                protoMessage.To,
                protoMessage.From,
                protoMessage.Id,
                Enum.Parse<MessageTypeSigr>(protoMessage.Type.ToString()),
                protoMessage.Data);

            instance.Metadata = protoMessage.Metadata.Any() ? protoMessage.Metadata.ToDictionary() : new Dictionary<string, string>();
            instance.Headers = protoMessage.Headers.Any() ? protoMessage.Headers.ToDictionary() : new Dictionary<string, string>();
            return instance;
        }

        public static implicit operator Message(MessageSigr message)
        {
            var protoMessage = new Message();
            protoMessage.To = message.To;
            protoMessage.From = message.From;
            protoMessage.Id = message.Id;
            protoMessage.Type = Enum.Parse<MessageType>(message.Type.ToString());
            protoMessage.Data = message.Data;
            protoMessage.Metadata.MergeFrom(message.Metadata);
            protoMessage.Headers.MergeFrom(message.Headers);
            return protoMessage;
        }
    }
}
