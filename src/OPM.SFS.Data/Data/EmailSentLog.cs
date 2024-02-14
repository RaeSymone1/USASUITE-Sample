using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class EmailSentLog
    {
        public int EmailSentLogID { get; set; }
        public string Subject { get; set; }
        public string Recipients { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }
    }
}
