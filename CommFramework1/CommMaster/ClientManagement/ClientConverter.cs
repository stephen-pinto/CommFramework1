namespace CommMaster.ClientManagement
{
    internal static class ClientConverter
    {
        public static Client ToClient(this RegisterationRequest request)
        {
            return new Client(
                request.ClientId,
                request.Name,
                request.Type,
                request.Address,
                request.Properties.ToDictionary(),
                DateTime.Now);
        }
    }
}
