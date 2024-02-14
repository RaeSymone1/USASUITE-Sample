using Azure.Core;
using Azure.Identity;
using MediatR;
using NUglify.JavaScript.Syntax;
using OPM.SFS.Data;
using OPM.SFS.Data.Migrations;
using OPM.SFS.Web.Controllers;
using OPM.SFS.Web.Shared;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
	public interface IStudentDashboardUpdateRule
	{
		Task<bool> CalculateDashboardFieldAsync(string value, SFS.Data.StudentInstitutionFunding record );
	}
	
}
