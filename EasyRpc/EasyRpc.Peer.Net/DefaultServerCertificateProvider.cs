using EasyRpc.Core.Util;

namespace EasyRpc.Peer.Net
{
    internal class DefaultServerCertificateProvider : ICertificateProvider
    {
        private string CertificatePath => Path.Combine(Environment.GetEnvironmentVariable("EASYRPC_TEST_CERT")!, "Server.crt");

        public string GetCertificateContent() => File.ReadAllText(CertificatePath);

        public string GetCertificatePath() => CertificatePath;

        public string GetKeyContent() => throw new NotImplementedException();

        public string GetKeyPath() => throw new NotImplementedException();
    }
}
