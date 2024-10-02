using EasyRpc.Core.Util;

namespace EasyRpc.Peer.Net
{
    internal class DefaultClientCertificateProvider : ICertificateProvider
    {
        private string CertificatePath => Path.Combine(Environment.GetEnvironmentVariable("EASYRPC_TEST_CERT")!, "Client.crt");

        private string KeyPath => Path.Combine(Environment.GetEnvironmentVariable("EASYRPC_TEST_CERT")!, "Client.key");

        public string GetCertificateContent() => File.ReadAllText(CertificatePath);

        public string GetCertificatePath() => CertificatePath;

        public string GetKeyContent() => File.ReadAllText(KeyPath);

        public string GetKeyPath() => KeyPath;
    }
}
