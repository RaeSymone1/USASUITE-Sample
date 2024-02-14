using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
	[Authorize(Roles = "AD")]
	public class SystemAdminModel : PageModel
	{
		[BindProperty]
		public SystemAdminViewModel Data { get; set; }

		[FromQuery(Name = "s")]
		public string IsSuccess { get; set; }

		private readonly IMediator _mediator;
		public SystemAdminModel(IMediator mediator) => _mediator = mediator;

		public async Task<IActionResult> OnGetAsync()
		{
			Data = await _mediator.Send(new Query() { Id = User.GetUserId() });
			if (!Data.IsValid)
			{
				return RedirectToPage("Home");
			}
			return Page();
		}

		public async Task<IActionResult> OnPostDashboardAsync()
		{
			await _mediator.Send(new Command() { Model = Data });
			return RedirectToPage("SystemAdmin", new { s = "true" });
		}

		public async Task<IActionResult> OnPostRemoveCertAsync()
		{
			await _mediator.Send(new CommandRemoveCert() { UserID = Data.UserID });
			return RedirectToPage("SystemAdmin", new { s = "true" });
		}

		public async Task<IActionResult> OnPostTogglePivAsync()
		{
			await _mediator.Send(new CommandTogglePIV() { UserID = Data.UserID });
			return RedirectToPage("SystemAdmin", new { s = "true" });
		}

		public class SystemAdminViewModelValidator : AbstractValidator<SystemAdminViewModel>
		{
			public SystemAdminViewModelValidator()
			{
				//RuleFor(x => x.Input).NotEmpty().Matches("^([1-9]|1[0-9]|2[0-5])$");
			}
		}

		public class Query : IRequest<SystemAdminViewModel>
		{
			public int Id { get; set; }
		}

		public class QueryHandler : IRequestHandler<Query, SystemAdminViewModel>
		{
			private readonly IConfiguration _appSettings;
			public QueryHandler(IConfiguration appSettings)
			{
				_appSettings = appSettings;
			}
			public Task<SystemAdminViewModel> Handle(Query request, CancellationToken cancellationToken)
			{
				string approvedIDs = _appSettings["General:SetDashboardDefaults"];
				List<string> _options = new List<string>() { "Reload Commitments for Dashboard", "Reload Commitments For Dashboard By Student ID", "Reload Commitments For Dashboard By UID List" };
				SystemAdminViewModel data = new();
				data.Options = new SelectList(_options);

				if (approvedIDs.Contains(request.Id.ToString()))
				{
					data.IsValid = true;
					return Task.FromResult(data);
				}
				data.IsValid = false;
				return Task.FromResult(data);
			}
		}

		public class Command : IRequest<bool>
		{
			public SystemAdminViewModel Model { get; set; }
		}

		public class CommandHandler : IRequestHandler<Command, bool>
		{
			private readonly IStudentDashboardLoader _loader;


			public CommandHandler(IStudentDashboardLoader loader, IConfiguration appSettings)
			{
				_loader = loader;
			}

			public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
			{
				if (request.Model.SelectedOption == "Reload Commitments for Dashboard")
					await _loader.LoadCommitmentsForDashboardByBatchesAsync(request.Model.Input, request.Model.BatchSize);
				else if (request.Model.SelectedOption == "Reload Commitments For Dashboard By Student ID")
					await _loader.LoadCommitmentsForDashboardByStudentID(request.Model.Input);
				else if (request.Model.SelectedOption == "Reload Student Dashboard - All")
					await _loader.LoadAsync(request.Model.Input);
				else if (request.Model.SelectedOption == "Reload Commitments For Dashboard By UID List")
					await _loader.LoadCommitmentsForDashboardByStudentUIDList(request.Model.Input);
				return true;
			}
		}

		public class CommandRemoveCert : IRequest<bool>
		{
			public int UserID { get; set; }
		}

		public class CommandRemoveCertHandler : IRequestHandler<CommandRemoveCert, bool>
		{
			private readonly ScholarshipForServiceContext _efDB;
			public CommandRemoveCertHandler(ScholarshipForServiceContext efDB) => _efDB = efDB;

			public async Task<bool> Handle(CommandRemoveCert request, CancellationToken cancellationToken)
			{
				var adminUser = await _efDB.AdminUsers.Where(m => m.AdminUserId == request.UserID).FirstOrDefaultAsync();
				adminUser.Certificate = null;
				adminUser.Issuer = null;
				adminUser.SerialNumber = null;
				adminUser.Subject = null;
				adminUser.SubjectAlternative = null;
				adminUser.Thumbprint = null;
				adminUser.ValidAfter = null;
				adminUser.ValidUntil = null;
				await _efDB.SaveChangesAsync();
				return true;
			}
		}
		public class CommandTogglePIV : IRequest<bool>
		{
			public int UserID { get; set; }
		}

		public class CommandTogglePIVHandler : IRequestHandler<CommandTogglePIV, bool>
		{
			private readonly ScholarshipForServiceContext _efDB;
			public CommandTogglePIVHandler(ScholarshipForServiceContext efDB) => _efDB = efDB;

			public async Task<bool> Handle(CommandTogglePIV request, CancellationToken cancellationToken)
			{
				var adminUser = await _efDB.AdminUsers.Where(m => m.AdminUserId == request.UserID).FirstOrDefaultAsync();
				adminUser.EnforcePIV = !adminUser.EnforcePIV;
				await _efDB.SaveChangesAsync();
				return true;
			}
		}

	}

	public class SystemAdminViewModel
	{
		public string Input { get; set; }
		public int UserID { get; set; }
		public bool IsValid { get; set; }
		public int BatchSize { get; set; }
		public SelectList Options { get; set; }
		public string SelectedOption { get; set; }
	}
}
