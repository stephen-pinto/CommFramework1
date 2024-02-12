namespace CommMaster.ClientManagement
{
    public record Peer(string Id, string Name, string Type, string Address, Dictionary<string, string> Properties, DateTime LastUpdate)
    {
    }
}
