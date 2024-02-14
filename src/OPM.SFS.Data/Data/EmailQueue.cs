using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
	public class EmailQueue
	{
		public int EmailQueueID { get; set; }
		public int PriorityID { get; set; }
		public string Token { get; set; }
		public string FromUri { get; set; }
		public string ToUri { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public DateTime QueueDate { get; set; }
		public bool IsMultiPart { get; set; }
		public string ReplyTo { get; set; }
		public string Sender { get; set; }
	}
}
