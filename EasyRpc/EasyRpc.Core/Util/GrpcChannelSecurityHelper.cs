using Grpc.Core;
using System.Security.Cryptography.X509Certificates;

namespace EasyRpc.Core.Util
{
    public static class GrpcChannelSecurityHelper
    {
        public static void SetAutoTrustedServerCertificates(HttpClientHandler handler, string certificateFilePath)
        {
            // Validate the server certificate with the root CA
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, _) =>
            {
                chain!.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                chain.ChainPolicy.CustomTrustStore.Add(new X509Certificate2(certificateFilePath));
                return chain.Build(cert!);
            };
        }

        public static void SetClientCertificates(HttpClientHandler handler, string pemCertFilePath, string keyFilePath)
        {
            var clientCert = X509Certificate2.CreateFromPemFile(pemCertFilePath, keyFilePath);
            handler.ClientCertificates.Add(clientCert);
        }

        public static SslServerCredentials GetSecureServerCredentials(string certificateFilePath, string keyFilePath)
        {
            List<KeyCertificatePair> certificates = [
                new KeyCertificatePair(File.ReadAllText(certificateFilePath), File.ReadAllText(keyFilePath))
            ];
            return new SslServerCredentials(certificates);
        }
    }
}
