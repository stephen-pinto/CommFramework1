using EasyRpc.Core.Base;
using EasyRpc.Core.Client;
using EasyRpc.Core.Util;

namespace EasyRpc.Master.PeerBase
{
    internal class GrpcPeerClientFactory : IPeerClientFactory
    {
        public IPeerService GetHandle(RegistrationRequest registerationRequest)
        {
            ICertificateProvider clientCertificateProvider = new DefaultClientCertificateProvider();
            HttpClientHandler handler = new HttpClientHandler();
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, clientCertificateProvider);
            GrpcChannelSecurityHelper.SetClientCertificates(handler, clientCertificateProvider);
            return new GrpcPeerClient(registerationRequest.Address, handler);
        }
    }
}
