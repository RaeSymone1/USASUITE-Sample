using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models.Admin
{
	public class DataManagementVM
	{
		
		public string ID { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Option { get; set; }
		public string Status { get; set; }
		public string Phase { get; set; }
		public string Placement { get; set; }
		public string DataGroup { get; set; }
	}
}
