namespace CommMaster.PeerClient
{
    public record Peer(string Id, string Name, string Type, string Address, Dictionary<string, string> Properties, DateTime LastUpdate)
    {
    }
}
