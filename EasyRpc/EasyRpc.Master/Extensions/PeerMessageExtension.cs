using EasyRpc.Types;

namespace EasyRpc.Master.Extensions
{
    internal static class PeerMessageExtension
    {
        public static Message ToMessage(this Message peerMessage)
        {
            //CHECK: Check performance of this
            var msg = new Message()
            {
                From = peerMessage.From,
                To = peerMessage.To,
                Data = peerMessage.Data,
                Id = peerMessage.Id,
                Type = (MessageType)(int)peerMessage.Type
            };

            msg.Metadata.Add(peerMessage.Metadata);
            msg.Headers.Add(peerMessage.Headers);

            return msg;
        }
    }
}
