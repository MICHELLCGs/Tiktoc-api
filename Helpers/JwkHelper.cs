namespace tiktoc_api.Helpers
{
    using System.Security.Cryptography;

    public class JwkHelper
    {
        public static (string publicKey, string privateKey) GenerateJwk()
        {
            using (var rsa = RSA.Create(2048))
            {
                return (
                    Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                    Convert.ToBase64String(rsa.ExportRSAPrivateKey())
                );
            }
        }
    }
}
