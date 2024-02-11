namespace CommServerLib.ClientManagement
{
    public record Client(string Id, string Name, string Type, string Address, string Port, Dictionary<string, string> Properties, DateTime LastUpdate)
    {
    }
}
