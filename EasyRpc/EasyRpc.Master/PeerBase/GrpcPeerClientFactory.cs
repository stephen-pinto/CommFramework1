using EasyRpc.Core.Base;
using EasyRpc.Core.Client;
using EasyRpc.Core.Util;

namespace EasyRpc.Master.PeerBase
{
    internal class GrpcPeerClientFactory : IPeerClientFactory
    {
        public IPeerService GetHandle(RegistrationRequest registerationRequest)
        {
            ICertificateProvider serverCertificateProvider = new DefaultServerCertificateProvider();
            ICertificateProvider clientCertificateProvider = new DefaultClientCertificateProvider();
            HttpClientHandler handler = new HttpClientHandler();
            //FIXME: Seems weird that we are using server certificate here should be client technically...
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, serverCertificateProvider);
            GrpcChannelSecurityHelper.SetClientCertificates(handler, clientCertificateProvider);
            return new GrpcPeerClient(registerationRequest.Address, handler);
        }
    }
}
