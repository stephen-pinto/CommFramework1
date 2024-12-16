namespace EasyRpc.Core.Client
{
    public record PeerInfo(string Id, string Name, string Type, string Address, Dictionary<string, string> Properties, DateTime Created);
}
