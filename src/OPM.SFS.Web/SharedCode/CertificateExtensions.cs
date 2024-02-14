using System.Security.Cryptography.X509Certificates;

namespace OPM.SFS.Web.SharedCode
{
    public static class CertificateExtensions
    {
        public static string SubjectAlternativeName(this X509Certificate2 cert)
        {
            var subjectAltName = cert.Extensions["Subject Alternative Name"];
            if (subjectAltName == null)
            {
                return string.Empty;
            }
            return subjectAltName.Format(true);
        }

    }
}
