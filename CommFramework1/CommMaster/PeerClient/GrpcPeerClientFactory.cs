using CommMaster.Util;
using CommServices.CommMaster;

namespace CommMaster.PeerClient
{
    internal class GrpcPeerClientFactory : IPeerClientFactory
    {
        public IPeerClient GetHandle(RegisterationRequest registerationRequest)
        {
            HttpClientHandler handler = new HttpClientHandler();
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, CommonConstants.ServerCertificatePath);
            GrpcChannelSecurityHelper.SetClientCertificates(handler, CommonConstants.ClientCertificatePath, CommonConstants.ClientKeyPath);
            return new GrpcPeerClient(registerationRequest.Address, handler);
        }
    }
}
