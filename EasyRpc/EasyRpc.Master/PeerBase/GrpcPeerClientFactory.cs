using EasyRpc.Core.Client;
using EasyRpc.Core.Util;

namespace EasyRpc.Master.PeerBase
{
    internal class GrpcPeerClientFactory : IPeerClientFactory
    {
        public IPeerClient GetHandle(RegistrationRequest registerationRequest)
        {
            ICertificateProvider serverCertificateProvider = new DefaultServerCertificateProvider();
            ICertificateProvider clientCertificateProvider = new DefaultClientCertificateProvider();
            HttpClientHandler handler = new HttpClientHandler();
            //TODO: Move this
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, serverCertificateProvider);
            GrpcChannelSecurityHelper.SetClientCertificates(handler, clientCertificateProvider);
            return new GrpcPeerClient(registerationRequest.Address, handler);
        }
    }
}
