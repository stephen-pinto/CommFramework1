using EasyRpc.Core.Client;
using EasyRpc.Core.Util;

namespace EasyRpc.Master.PeerBase
{
    internal class GrpcPeerClientFactory : IPeerClientFactory
    {
        public IPeerClient GetHandle(RegistrationRequest registerationRequest)
        {
            HttpClientHandler handler = new HttpClientHandler();
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, CommonConstants.ServerCertificatePath);
            GrpcChannelSecurityHelper.SetClientCertificates(handler, CommonConstants.ClientCertificatePath, CommonConstants.ClientKeyPath);
            return new GrpcPeerClient(registerationRequest.Address, handler);
        }
    }
}
