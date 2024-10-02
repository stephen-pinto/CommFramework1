using EasyRpc.Core.Client;
using EasyRpc.Core.Util;

namespace EasyRpc.Master.PeerBase
{
    internal class GrpcPeerClientFactory : IPeerClientFactory
    {
        public IPeerClient GetHandle(RegistrationRequest registerationRequest)
        {
            var certDir = Environment.GetEnvironmentVariable("EASYRPC_TEST_CERT");
            HttpClientHandler handler = new HttpClientHandler();
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, Path.Combine(certDir!, CommonConstants.ServerCertificateFile));
            GrpcChannelSecurityHelper.SetClientCertificates(handler,
                Path.Combine(certDir!, CommonConstants.ClientCertificateFile),
                Path.Combine(certDir!, CommonConstants.ClientKeyFile));
            return new GrpcPeerClient(registerationRequest.Address, handler);
        }
    }
}
