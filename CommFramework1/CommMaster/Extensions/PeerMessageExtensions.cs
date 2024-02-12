//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CommMaster.Extensions
//{
//    internal static class PeerMessageExtensions
//    {
//        public static Message ToMessage(this CommPeer.Message peerMessage)
//        {
//            //CHECK: Check performance of this
//            var msg = new Message()
//            {
//                From = peerMessage.From,
//                To = peerMessage.To,
//                Data = peerMessage.Data,
//                Id = peerMessage.Id,                
//                Type = (MessageType)(int)peerMessage.Type
//            };

//            msg.Metadata.Add(peerMessage.Metadata);
//            msg.Headers.Add(peerMessage.Headers);

//            return msg;
//        }
//    }
//}
