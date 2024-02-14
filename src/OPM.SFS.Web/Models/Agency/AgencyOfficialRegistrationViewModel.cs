using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models.Agency
{
	public class AgencyOfficialRegistrationViewModel
	{
		public int AgencyType { get; set; }
		public SelectList AgencyTypeList { get; set; }
		public int Agency { get; set; }
		public SelectList AgencyList { get; set; }
		public int SubAgency { get; set; }
		public SelectList SubAgencyList { get; set; }
		public string  FirstName { get; set; }
		public string LastName { get; set; }
		public int Role { get; set; }
		public SelectList RoleList { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public int State { get; set; }
		public SelectList StateList { get; set; }
		public string PostalCode { get; set; }
		public string Phone { get; set; }
		public string Extension { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public string Website { get; set; }
		public string DisplayPermission { get; set; }
        public int StateFilter { get; set; }

        public class AgencyListDTO
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string ApprFlow { get; set; }
		}

	}
}
