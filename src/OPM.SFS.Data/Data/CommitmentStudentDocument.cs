using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class CommitmentStudentDocument
    {
        public int CommitmentStudentDocumentID { get; set; }
        public int StudentCommitmentID { get; set; }
        public int StudentDocumentID { get; set; }
        public DateTime? DateAdded { get; set; }
        public virtual StudentCommitment StudentCommitment { get; set; }
        public virtual StudentDocument StudentDocument { get; set; }    
    }
}
