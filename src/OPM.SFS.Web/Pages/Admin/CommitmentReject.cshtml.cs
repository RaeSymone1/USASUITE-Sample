using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class CommitmentRejectModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public AdminCommitmentRejectVM Data { get; set; }

        [FromQuery(Name = "cid")]
        public int CommitId { get; set; } = 0;

        public CommitmentRejectModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { CommitID = CommitId });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                 await _mediator.Send(new Command() {  Model = Data });
            }

            return RedirectToPage("/Admin/CommitmentList");
        }

        public class Query : IRequest<AdminCommitmentRejectVM>
        {
            public int CommitID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminCommitmentRejectVM>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICryptoHelper _crypto;
            private readonly ICacheHelper _cache;


            public QueryHandler(ScholarshipForServiceContext efDB, ICryptoHelper crypto, ICacheHelper cache)
            {
                _efDB = efDB;
                _crypto = crypto;
                _cache = cache;
            }
            public async Task<AdminCommitmentRejectVM> Handle(Query request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();

                var commitData = await _efDB.StudentCommitments
                    .Where(m => m.StudentCommitmentId == request.CommitID)
                    .Select(m => new
                    {
                        CommitID = m.StudentCommitmentId,
                        FirstName = m.Student.FirstName,
                        LastName = m.Student.LastName,
                        SSN = m.Student.Ssn,
                        Agency = m.Agency.Name,
                        JobTitle = m.JobTitle,
                        CommitmentType = m.CommitmentType.Name
                    })
                    .FirstOrDefaultAsync();

                AdminCommitmentRejectVM vm = new();
                var ssn = _crypto.Decrypt(commitData.SSN, GlobalConfigSettings);
                vm.CommitID = commitData.CommitID;
                vm.FormattedSSN = $"xxx-xx-{ssn.Substring(ssn.Length - 4)}";
                vm.FirstName = commitData.FirstName;
                vm.LastName = commitData.LastName;
                vm.Agency = commitData.Agency;
                vm.JobTitle = commitData.JobTitle;
                vm.CommitType = commitData.CommitmentType;
                return vm;
            }
        }

        public class Command : IRequest<bool>
        {
            public AdminCommitmentRejectVM Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public CommandHandler(ScholarshipForServiceContext efDB)
            {
                _efDB = efDB;
            }
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var toUpdate = await _efDB.StudentCommitments.Where(m => m.StudentCommitmentId == request.Model.CommitID).FirstOrDefaultAsync();
                toUpdate.RejectNote = request.Model.RejectNote;
                await _efDB.SaveChangesAsync();
                return true;
            }
        }
    }
}
