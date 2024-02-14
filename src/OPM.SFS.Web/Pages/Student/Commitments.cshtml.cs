using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Infrastructure.Extensions;

namespace OPM.SFS.Web.Pages.Student
{

    [Authorize(Roles = "ST")]
    [IsProfileCompleteFilter()]
    public class CommitmentsModel : PageModel
    {
        [BindProperty]
        public List<Commmitment> Data { get; set; }

        private readonly IMediator _mediator;
        public CommitmentsModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new CommitmentQuery() { Id = User.GetUserId() });
        }      
    }   

    public class Commmitment
    {
        public string Action { get; set; }
        public string CommitmentType { get; set; }
        public string StartDate { get; set; }
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string Manager { get; set; }
        public string Agency { get; set; }
        public string StatusDisplay { get; set; }
        public string StatusCode { get; set; }
        public string ManagerEmail { get; set; }
        public string StatusDescription { get; set; }
    }


    public class CommitmentQuery : IRequest<List<Commmitment>>
    {
        public int Id { get; set; }
    }

    public class GetCommitmentsHandler : IRequestHandler<CommitmentQuery, List<Commmitment>>
    {
        private readonly ScholarshipForServiceContext _efDB;
        public GetCommitmentsHandler(ScholarshipForServiceContext efDB) 
        {
            _efDB = efDB;
        }

        public async Task<List<Commmitment>> Handle(CommitmentQuery request, CancellationToken cancellationToken)
        {
            //List<Commmitment> _allCommitments = new List<Commmitment>();
            var _allCommitments = await _efDB.StudentCommitments
                        .Where(m => m.StudentId == request.Id)
                        .Where(m => m.Agency.IsDisabled == false)
                        .Where(m => m.IsDeleted == false)
                        .Select(p => new Commmitment()
                        {
                            Id = p.StudentCommitmentId,
                            CommitmentType = p.CommitmentType.Name,
                            Agency = p.Agency.Name,
                            JobTitle = p.JobTitle,
                            StartDate = p.StartDate.Value.ToShortDateString(),
                            Manager = String.IsNullOrWhiteSpace(p.SupervisorContact.LastName) ? "" : $"{p.SupervisorContact.LastName},{p.SupervisorContact.FirstName}",
                            StatusDisplay = p.CommitmentStatus.StudentDisplay,
                            StatusCode = p.CommitmentStatus.Value,
                            ManagerEmail = p.SupervisorContact.Email,
                            StatusDescription = p.CommitmentStatus.Description
                        }).OrderByDescending(m => m.Id)
                        .ToListAsync();
            return _allCommitments;
        }
    }
}
