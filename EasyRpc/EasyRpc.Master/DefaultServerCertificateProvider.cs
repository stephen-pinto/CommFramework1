using EasyRpc.Core.Util;

namespace EasyRpc.Master
{
    internal class DefaultServerCertificateProvider : ICertificateProvider
    {
        private string CertificatePath => Path.Combine(Environment.GetEnvironmentVariable("EASYRPC_TEST_CERT")!, "Server.crt");

        private string KeyPath => Path.Combine(Environment.GetEnvironmentVariable("EASYRPC_TEST_CERT")!, "Server.key");

        public string GetCertificateContent() => File.ReadAllText(CertificatePath);

        public string GetCertificatePath() => CertificatePath;

        public string GetKeyContent() => File.ReadAllText(KeyPath);

        public string GetKeyPath() => KeyPath;
    }
}
