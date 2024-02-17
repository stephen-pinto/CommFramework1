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
    }
}