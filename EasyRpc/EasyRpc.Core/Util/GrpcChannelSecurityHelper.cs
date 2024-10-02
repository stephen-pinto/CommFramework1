using Grpc.Core;
using System.Security.Cryptography.X509Certificates;

namespace EasyRpc.Core.Util
{
    public static class GrpcChannelSecurityHelper
    {
        public static void SetAutoTrustedServerCertificates(HttpClientHandler handler, ICertificateProvider certificateProvider)
        {
            // Validate the server certificate with the root CA
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, _) =>
            {
                ArgumentNullException.ThrowIfNull(chain, nameof(chain));

                chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                chain.ChainPolicy.CustomTrustStore.Add(new X509Certificate2(certificateProvider.GetCertificatePath()));
                return chain.Build(cert!);
            };
        }

        public static void SetClientCertificates(HttpClientHandler handler, ICertificateProvider certificateProvider)
        {
            var clientCert = X509Certificate2.CreateFromPemFile(
                certificateProvider.GetCertificatePath(),
                certificateProvider.GetKeyPath());

            handler.ClientCertificates.Add(clientCert);
        }

        public static SslServerCredentials GetSecureServerCredentials(ICertificateProvider certificateProvider)
        {
            List<KeyCertificatePair> certificates = [
                new KeyCertificatePair(certificateProvider.GetCertificateContent(), certificateProvider.GetKeyContent())
            ];
            return new SslServerCredentials(certificates);
        }
    }
}
