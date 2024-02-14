using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class RegistrationsModel : PageModel
    {
        [BindProperty]
        public AdminRegisterApprovalViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public RegistrationsModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public class Query : IRequest<AdminRegisterApprovalViewModel>
        {

        }

        public class QueryHandler : IRequestHandler<Query, AdminRegisterApprovalViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }

            public async Task<AdminRegisterApprovalViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var allStatus = await _cache.GetProfileStatusAsync();
                var pendingID = allStatus.Where(m => m.Name == "Pending").Select(m => m.ProfileStatusID).FirstOrDefault();
                var pendingStudents = await _efDB.Students.Where(m => m.ProfileStatusID == pendingID).CountAsync();
                var pendingAOs = await _efDB.AgencyUsers.Where(m => m.ProfileStatusID == pendingID).CountAsync();
                var pendingPIs = await _efDB.AcademiaUsers.Where(m => m.ProfileStatusID == pendingID).CountAsync();

                AdminRegisterApprovalViewModel model = new();
                model.StudentPendingApproval = pendingStudents;
                model.PIPendingApproval = pendingPIs;
                model.AOPendingApproval = pendingAOs;
                return model;
            }
        }
    }
}
