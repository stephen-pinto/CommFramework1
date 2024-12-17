﻿using EasyRpc.Core.Base;
using EasyRpc.Plugin.SignalR.Types;

namespace EasyRpc.Plugin.SignalR.Interfaces
{
    public interface ISigrPeerClientStore
    {
        IPeerService GetClient(string connectionId);
        IPeerService AddNewRegisteredClient(string connectionId, RegistrationRequestSigr registration);
        void RemoveClient(string connectionId);
    }
}
