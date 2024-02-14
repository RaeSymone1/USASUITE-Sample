using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Data
{
    public class DocumentScanQueue
    {
        public int DocumentScanQueueId { get; set; }
        public string FilePath { get; set; }
        public DateTime? DateQueued { get; set; }
        public string QueuedBy { get; set; }
    }
}
