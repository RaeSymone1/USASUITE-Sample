using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class CertificateStaging
    {
        public Guid CertificateStagingID { get; set; }
        public byte[] Thumbprint { get; set; }
        public string Issuer { get; set; }
        public string SerialNumber { get; set; }
        public string Subject { get; set; }
        public string SubjectAlternative { get; set; }
        public DateTime? ValidAfter { get; set; }
        public DateTime? ValidUntil { get; set; }
        public byte[] Certificate { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
