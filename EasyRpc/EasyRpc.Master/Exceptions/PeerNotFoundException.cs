namespace EasyRpc.Master.Exceptions
{
    public class PeerNotFoundException : Exception
    {
        public PeerNotFoundException(string message) : base(message)
        {
        }

        public static void ThrowIfNullOrEmpty(string peerId, string? message = null)
        {
            if (string.IsNullOrEmpty(peerId))
            {
                throw new PeerNotFoundException(message ?? "Peer id is null or empty");
            }
        }
    }
}