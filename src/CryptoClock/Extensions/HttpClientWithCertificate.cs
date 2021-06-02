using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CryptoClock.Extensions
{
    class HttpClientWithCertificate : HttpClient
    {
        public HttpClientWithCertificate(string certificateFile) : base(CreateHttpClientHandler(new X509Certificate2(certificateFile)))
        {
        }


        public HttpClientWithCertificate(X509Certificate2 certificate) : base(CreateHttpClientHandler(certificate))
        {
        }

        private static HttpClientHandler CreateHttpClientHandler(X509Certificate2 certificate)
        {
            var certificateHash = certificate.GetCertHashString();

            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, serverCertificate, __, errors) =>
                    errors == SslPolicyErrors.None ||
                    serverCertificate.GetCertHashString() == certificateHash
            };
        }
    }
}