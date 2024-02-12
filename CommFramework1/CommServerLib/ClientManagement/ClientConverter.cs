using CommServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommServerLib.ClientManagement
{
    internal static class ClientConverter
    {
        public static Client ToClient(this RegisterationRequest request)
        {
            return new Client(request.ClientId, request.Name, request.Type, request.Address, request.Port, request.Properties.ToDictionary(), DateTime.Now);
        }
    }
}