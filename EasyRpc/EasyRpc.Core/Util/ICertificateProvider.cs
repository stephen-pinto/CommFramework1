namespace EasyRpc.Core.Util
{
    public interface ICertificateProvider
    {
        string GetCertificatePath();

        string GetKeyPath();

        string GetCertificateContent();

        string GetKeyContent();
    }
}
