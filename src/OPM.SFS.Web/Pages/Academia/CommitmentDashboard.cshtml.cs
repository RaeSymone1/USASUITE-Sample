using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models.Academia;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]
    public class CommitmentDashboardModel : PageModel
    {
        [BindProperty]
        public AcademiaCommitmentDashboardViewModel Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StatusID { get; set; }
        [FromQuery(Name = "sd")]
        public string Startdate { get; set; }
        [FromQuery(Name = "ed")]
        public string Enddate { get; set; }


        private readonly IMediator _mediator;

        public CommitmentDashboardModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StatusID = StatusID, Start = Startdate, End = Enddate, UserID = User.GetUserId() });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }
                
            var data = await _mediator.Send(new Command() { Model = Data });
            return Redirect($"/Academia/CommitmentDashboard?sid={data.StatusID}&sd={data.Startdate}&ed={data.Enddate}");
        }

        public class AdminCommitmentSummaryValidator : AbstractValidator<AcademiaCommitmentDashboardViewModel>
        {
            public AdminCommitmentSummaryValidator()
            {

                RuleFor(request => request.ApprovalStatus).NotEmpty();
                RuleFor(request => request.LastApprovedStartdate).Matches("^[^><&]+$").WithName("Start date");
                RuleFor(request => request.LastApprovedStartdate).Must(IsValidDate).WithMessage("Start date is invalid date format.");
                RuleFor(request => request.LastApprovedEndDate).Matches("^[^><&]+$").WithName("End date");
                RuleFor(request => request.LastApprovedEndDate).Must(IsValidDate).WithMessage("End date is invalid date format.");

            }

            protected bool IsValidDate(string date)
            {
                if (string.IsNullOrWhiteSpace(date)) return true;
                if (DateTime.TryParse(date, out _)) return true;
                return false;
            }
        }

        public class Query : IRequest<AcademiaCommitmentDashboardViewModel>
        {
            public int StatusID { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public int UserID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AcademiaCommitmentDashboardViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<AcademiaCommitmentDashboardViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AcademiaCommitmentDashboardViewModel model = new();
                model.CommitmentsByAgencyType = new();
                model.CommitmentsByType = new();

                var institutionID = await _db.AcademiaUsers.Where(m => m.AcademiaUserId == request.UserID)
                   .Select(m => m.InstitutionID).FirstOrDefaultAsync();

				var allInstitutionIds = await _db.Institutions.Where(m => m.InstitutionId.Equals(institutionID) || m.ParentInstitutionID.Equals(institutionID)).Select(m => m.InstitutionId).ToListAsync();

				var statusOptions = await _cache.GetCommitmentStatusAsync();
                var commitmentTypes = await _cache.GetCommitmentTypeAsync();
                var agencyTypes = await _cache.GetAgencyTypesAsync();
                statusOptions = statusOptions.Where(m => m.Code != CommitmentProcessConst.Incomplete && m.IsDisabled == false).ToList();


                var statusID = statusOptions.Where(m => m.Code == CommitmentProcessConst.Incomplete).Select(m => m.CommitmentStatusID).FirstOrDefault();
                model.ApprovalStatusList = new SelectList(statusOptions, nameof(CommitmentStatus.CommitmentStatusID), nameof(CommitmentStatus.AdminDisplay));
                model.ReportDescription = "Summary of all submitted commitments";


                var totalCommitments = await _db.StudentCommitments.Include(m => m.CommitmentStatus).Include(m => m.Student)
                    .Where(m => m.CommitmentStatus.CommitmentStatusID != statusID && m.CommitmentStatus.Code != CommitmentProcessConst.Incomplete)
                    .Where(m => allInstitutionIds.Contains(m.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value))
                    .Where(m => m.Agency.IsDisabled == false)
					.Where(m => !m.IsDeleted)
					.WhereIf(request.StatusID > 0, x => x.CommitmentStatus.CommitmentStatusID == request.StatusID)
                    .WhereIf(!string.IsNullOrWhiteSpace(request.Start) && !string.IsNullOrWhiteSpace(request.End),
                        (x => x.DateApproved >= Convert.ToDateTime(request.Start) && x.DateApproved < Convert.ToDateTime(request.End)))
                    .AsNoTracking()
                    .CountAsync();


                var commitmentsByAgency = await _db.StudentCommitments
                    .Include(m => m.CommitmentStatus)
                    .Include(m => m.Student)
                    .Include(m => m.Agency).ThenInclude(m => m.AgencyType)
                    .Where(m => m.Agency.IsDisabled == false)
					.Where(m => !m.IsDeleted)
					.Where(m => m.CommitmentStatusId != statusID && m.CommitmentStatus.Code != CommitmentProcessConst.Incomplete)
                    .Where(m => allInstitutionIds.Contains(m.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value))
                    .WhereIf(request.StatusID > 0, x => x.CommitmentStatus.CommitmentStatusID == request.StatusID)
                    .WhereIf(!string.IsNullOrWhiteSpace(request.Start) && !string.IsNullOrWhiteSpace(request.End),
                        (x => x.DateApproved >= Convert.ToDateTime(request.Start) && x.DateApproved < Convert.ToDateTime(request.End)))
                    .AsNoTracking()
                    .GroupBy(m => m.Agency.AgencyType.AgencyTypeId).Select(x => new
                    {
                        ID = x.Key,
                        Count = x.Count()
                    }).ToListAsync();

                var commitmentsByCommitType = await _db.StudentCommitments.Include(m => m.CommitmentStatus).Include(m => m.Student)
                    .Where(m => m.CommitmentStatusId != statusID && m.CommitmentStatus.Code != CommitmentProcessConst.Incomplete)
                    .Where(m => allInstitutionIds.Contains(m.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value))
                    .Where(m => m.Agency.IsDisabled == false)
					.Where(m => !m.IsDeleted)
					.WhereIf(request.StatusID > 0, x => x.CommitmentStatus.CommitmentStatusID == request.StatusID)
                    .WhereIf(!string.IsNullOrWhiteSpace(request.Start) && !string.IsNullOrWhiteSpace(request.End),
                        (x => x.DateApproved >= Convert.ToDateTime(request.Start) && x.DateApproved < Convert.ToDateTime(request.End)))
                    .AsNoTracking()
                    .GroupBy(m => m.CommitmentType.CommitmentTypeId).Select(x => new
                    {
                        ID = x.Key,
                        Count = x.Count()
                    }).ToListAsync();


                model.TotalCommitments = totalCommitments;

                foreach (var agencyType in commitmentsByAgency)
                {
                    CommitmentsByType c = new CommitmentsByType()
                    {
                        TypeID = agencyType.ID,
                        TypeName = agencyTypes.Where(m => m.AgencyTypeId == agencyType.ID).FirstOrDefault().Name,
                        Total = agencyType.Count,
                        Percentage = Math.Round(((double)agencyType.Count / (double)totalCommitments) * 100, 2)

                    };
                    model.CommitmentsByAgencyType.Add(c);
                }


                foreach (var status in commitmentsByCommitType)
                {
                    CommitmentsByType c = new CommitmentsByType()
                    {
                        TypeID = status.ID,
                        TypeName = commitmentTypes.Where(m => m.CommitmentTypeId == status.ID).FirstOrDefault().Name,
                        Total = status.Count,
                        Percentage = Math.Round((double)status.Count / (double)totalCommitments, 2)

                    };
                    model.ApprovalStatus = request.StatusID;
                    model.LastApprovedStartdate = request.Start;
                    model.LastApprovedEndDate = request.End;
                    model.CommitmentsByType.Add(c);
                }

                return model;
            }
        }

        public class QuerySelectList : IRequest<AcademiaCommitmentDashboardViewModel>
        {
            public AcademiaCommitmentDashboardViewModel Model { get; set; }
        }

        public class QuerySelectListHandler : IRequestHandler<QuerySelectList, AcademiaCommitmentDashboardViewModel>
        {
            private readonly ICacheHelper _cache;

            public QuerySelectListHandler(ICacheHelper cache)
            {
                _cache = cache;
            }
            public async Task<AcademiaCommitmentDashboardViewModel> Handle(QuerySelectList request, CancellationToken cancellationToken)
            {
                var statusOptions = await _cache.GetCommitmentStatusAsync();
                var validStatusOptions = statusOptions.Where(m => m.Code != CommitmentProcessConst.Incomplete && m.IsDisabled == false).ToList();
                request.Model.ApprovalStatusList = new SelectList(validStatusOptions, nameof(CommitmentStatus.CommitmentStatusID), nameof(CommitmentStatus.AdminDisplay));
                return request.Model;

            }
        }

        public class Command : IRequest<SearchFiltersViewModel>
        {
            public AcademiaCommitmentDashboardViewModel Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, SearchFiltersViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public CommandHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }
            public async Task<SearchFiltersViewModel> Handle(Command request, CancellationToken cancellationToken)
            {
                return new SearchFiltersViewModel()
                {
                    StatusID = request.Model.ApprovalStatus,
                    Startdate = request.Model.LastApprovedStartdate,
                    Enddate = request.Model.LastApprovedEndDate
                };
            }
        }
    }
}
