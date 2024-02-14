using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Web.Models;
using OPM.SFS.Core.DTO;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    [IsProfileCompleteFilter()]
    public class CommitmentVerificationModel : PageModel
    {
        [BindProperty]
        public CommitmentVerificationViewModel Data { get; set; }

        private readonly IMediator _mediator;
        public CommitmentVerificationModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new CommitmentVerificationQuery() { Id = User.GetUserId() });
        }
    }

    public class CommitmentVerificationQuery : IRequest<CommitmentVerificationViewModel>
    {
        public int Id { get; set; }
    }

    public class GetCommitmentsVerificationHandler : IRequestHandler<CommitmentVerificationQuery, CommitmentVerificationViewModel>
    {        
        private readonly IStudentRepository _repo;
        public GetCommitmentsVerificationHandler(IStudentRepository repo)
        {           
            _repo = repo;
        }

        public async Task<CommitmentVerificationViewModel> Handle(CommitmentVerificationQuery request, CancellationToken cancellationToken)
        {          

            var mainData = await _repo.GetCommitmentVerificationData(request.Id);
            var commitmentData = await _repo.GetCommitmentVerificationDetailsData(request.Id);
            
            CommitmentVerificationViewModel serviceObligationModel = new CommitmentVerificationViewModel();
            serviceObligationModel.Commitments = new();
            serviceObligationModel.TotalServiceObligation = string.IsNullOrWhiteSpace(mainData.ServiceOwed) ? "N/A" : $"{mainData.ServiceOwed} Years";
            serviceObligationModel.NextVerificationDueDate = GetNextVerificationDueDate(mainData,commitmentData);
            foreach (var commitment in commitmentData.Take(5))
            {
                serviceObligationModel.Commitments.Add(new CommitmentVerificationViewModel.CommitmentDetails()
                {
                    Id = commitment.ID,
                    AgencyName = commitment.Agency,
                    JobTitle = commitment.JobTitle,
                    StartDate = !commitment.StartDate.HasValue ? "N/A" : commitment.StartDate.Value.ToShortDateString(),
                    CommitmentType = commitment.Type,
                    CommitmentStatus = commitment.Status,
                    EVFStatus = commitment.EVFStatus,
                    EVFDateSubmitted = commitment.EVFDateSubmitted,
                    ShowAddVerification = commitment.Status == "Approved" && commitment.Type == "Postgraduate" ? true : false,
                  
                });
            }
            return serviceObligationModel;
        }

        public string GetNextVerificationDueDate(CommitmentVerificationDTO mainData, List<CommitmentVerificationDetailsDTO> commitmentData)
        {
            var NextVerificationDueDate = "N/A";
            bool hasPostgraduate = commitmentData.Take(5).Any(commitment => commitment.Type == "Postgraduate");


            return !hasPostgraduate ? NextVerificationDueDate
                : !string.IsNullOrEmpty(mainData.PGVerificationOneDue) && string.IsNullOrEmpty(mainData.PGVerificationOneComplete) ? mainData.PGVerificationOneDue
                    : !string.IsNullOrEmpty(mainData.PGVerificationTwoDue) && string.IsNullOrEmpty(mainData.PGVerificationTwoComplete) ? mainData.PGVerificationTwoDue
                        : !string.IsNullOrEmpty(mainData.SOCDueDate) ? mainData.SOCDueDate
                            : NextVerificationDueDate;
        }
    }
}
