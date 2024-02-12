using CommClient;

namespace CommClientBase
{
    public interface IClientHandler
    {
        CommService.CommServiceClient Client { get; }

        void Start(string address);
    }
}
