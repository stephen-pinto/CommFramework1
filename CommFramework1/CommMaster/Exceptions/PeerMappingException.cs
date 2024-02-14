using System.Runtime.Serialization;

namespace CommMaster.Exceptions
{
    [Serializable]
    internal class PeerMappingException : Exception
    {
        public PeerMappingException()
        {
        }

        public PeerMappingException(string? message) : base(message)
        {
        }

        public PeerMappingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PeerMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}