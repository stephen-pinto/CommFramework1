namespace EasyRpc.Master.Exceptions
{
    public class PeerNotFoundException : Exception
    {
        public PeerNotFoundException(string message) : base(message)
        {
        }
    }
}
