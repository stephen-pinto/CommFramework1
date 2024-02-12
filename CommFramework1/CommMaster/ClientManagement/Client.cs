namespace CommMaster.ClientManagement
{
    public record Client(string Id, string Name, string Type, string Address, Dictionary<string, string> Properties, DateTime LastUpdate)
    {
    }
}
