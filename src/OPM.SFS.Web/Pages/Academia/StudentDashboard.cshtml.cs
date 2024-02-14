using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUglify.Helpers;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Admin.StudentDashboardModel;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]
    [IgnoreAntiforgeryToken] //THIS IS JUST FOR TESTING!! NEED TO FIX ASAP!!!!

    public class StudentDashboardModel : PageModel
    {
        [BindProperty]
        public ReferenceDataViewModel Data { get; set; }
        private readonly IMediator _mediator;

        public StudentDashboardModel(IMediator mediator) => _mediator = mediator;
        public void OnGet()
        {
        }

        public JsonResult OnGetAllStudents()
        {
            var data = _mediator.Send(new JsonQuery() { UserID = Convert.ToInt32(User.GetUserId()) }).Result;
            return new JsonResult(data);
        }

        public async Task<JsonResult> OnGetAllReferenceData()
        {
            Data = await _mediator.Send(new Query() { });
            return new JsonResult(Data);
        }
        public class Query : IRequest<ReferenceDataViewModel>
        {
        }
        public class QueryHandler : IRequestHandler<Query, ReferenceDataViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IReferenceDataRepository _repo;
            private readonly IFeatureManager _featureManager;

            public QueryHandler(ICacheHelper cache, ScholarshipForServiceContext efDB, IReferenceDataRepository repo, IFeatureManager featureManager)
            {
                _efDB = efDB;
                _repo = repo;
                _featureManager = featureManager;
            }

            public async Task<ReferenceDataViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                ReferenceDataViewModel model = new();
                var statusoption = await _repo.GetStatusOptionsAsync();
                var extensiontypelist = await _repo.GetExtensionTypeAsync();
                var citizenship = await _repo.GetCitizenshipAsync();
                var contracts = await _repo.GetContractAsync();
                var institution = await _repo.GetInstitutionsAsync();
                var degree = await _repo.GetDegreesAsync();
                var major = await _repo.GetDisciplinesAsync();
                var year = await _repo.GetProgramYearsAsync();
                var session = await _repo.GetSessionsAsync();

                model.Institutions = institution.Select(x => x.Name).ToList();
                model.Citizenship = citizenship.Select(x => x.Value).ToList();
                model.ExtensionTypes = extensiontypelist.Select(x => x.Extension).ToList();
                model.Contract = contracts.Select(m => m.Name).ToList();
                var followupTypeOption = await _repo.GetFollowUpTypeOptionsAsync();
                model.FollowUpType = followupTypeOption.Select(x => x.Name).ToList();
                model.Years = year.Select(x => x.Name).ToList();
                model.Sessions = session.Select(x => x.Name).ToList();
                model.Degrees = degree.Select(x => x.Name).ToList();
                model.Majors = major.Select(x => x.Name).ToList();
                model.IsEnabledOnSite = await _featureManager.IsEnabledSiteWideAsync("UnregisteredStudentFlow");
                model.StatusOptionLookup = new();

                var statusOptionKeys = statusoption.Where(m => m.IsDeleted == false).Select(m => m.Option).Distinct().ToList();

                foreach (var option in statusOptionKeys)
                {
                    var statusByOption = statusoption.Where(m => m.Option == option && m.IsDeleted == false).Select(m => m.Status).ToList();
                    model.StatusOptionLookup.Add(option, statusByOption);
                }
                return model;

            }
        }

        public class JsonQuery : IRequest<IEnumerable<StudentDashboardViewModel>>
        {
            public int UserID { get; set; }
        }

        public class JsonQueryHandler : IRequestHandler<JsonQuery, IEnumerable<StudentDashboardViewModel>>
        {

            private readonly IStudentRepository _repo;
            private readonly IStudentDashboardService _dashboardService;
            private readonly IFeatureManager _featuremanager;

            public JsonQueryHandler(IStudentRepository repo, IStudentDashboardService dashService, IFeatureManager featuremanager)
            {
                _dashboardService = dashService;
                _repo = repo;
                _featuremanager = featuremanager;
            }
            public async Task<IEnumerable<StudentDashboardViewModel>> Handle(JsonQuery request, CancellationToken cancellationToken)
            {
                var studentData = await _repo.GetAllStudentsForPIDashboardAsync(request.UserID);
                List<StudentDashboardViewModel> model = new();

                foreach (var student in studentData)
                {
                    model.Add(new StudentDashboardViewModel()
                    {
                        ID = student.StudentUID.ToString(),
                        StudentID = student.StudentID.ToString(),
                        Studentname = $"{student.Firstname} {student.LastName}",
                        EditUrl = $"/academia/StudentProfileEdit?sid={student.StudentID}",
                        Institution = student.Institution,
                        InstitutionID = student.InstitutionID.ToString(),
                        SchoolType = student.SchoolType,
                        EnrolledSession = student.EnrolledSession.IfNullOrWhiteSpace("N/A"),
                        EnrolledYear = student.EnrolledYear.IfNullOrWhiteSpace("N/A"),
                        Registered = student.ProfileStatus.Equals("Not Registered") ? "N" : "Y",
                        FundingEndSession = student.FundingEndSession.IfNullOrWhiteSpace("N/A"),
                        FundingEndYear = student.FundingEndYear.IfNullOrWhiteSpace("N/A"),
                        GraduationDateMonth = student.GraduationDate.Length > 0 ? Convert.ToDateTime(student.GraduationDate).ToString("MMM") : "",
                        GraduationDateYear = student.GraduationDate.Length > 0 ? Convert.ToDateTime(student.GraduationDate).Year.ToString() : "",
                        StatusOption = string.IsNullOrWhiteSpace(student.StatusOption) ? "N/A" : student.StatusOption,
                        ProgramPhase = string.IsNullOrWhiteSpace(student.ProgramPhase) ? "N/A" : student.ProgramPhase,
                        EmailAddress = student.EmailAddress,
                        Degree = student.Degree,
                        AltEmail = student.AltEmail.IfNullOrWhiteSpace("N/A"),
                        Major = student.Major,
                        InstitutionGroup = "TBD",
                        Minor = student.Minor.IfNullOrWhiteSpace("N/A"),
                        SecondDegreeMajor = student.SecondDegreeMajor.IfNullOrWhiteSpace("N/A"),
                        SecondDegreeMinor = student.SecondDegreeMinor.IfNullOrWhiteSpace("N/A"),
                        AcademicSchedule = student.AcademicSchedule,
                        Status = student.Status.IfNullOrWhiteSpace("N/A"),
                        ServiceOwed = student.ServiceOwed.HasValue ? student.ServiceOwed.Value.ToString() : "N/A",
                        Contract = student.Contract.IfNullOrWhiteSpace("N/A"),
                        DeferralAgreementReceived = "TBD",
                        INReported = student.InternshipReported.IfNullOrWhiteSpace("N/A"),
                        PGReported = student.PostGradReported.IfNullOrWhiteSpace("N/A"),
                        INAgencyType = student.InternshipAgencyType.IfNullOrWhiteSpace("N/A"),
                        INHDQAgencyName = student.InternshipAgencyName.IfNullOrWhiteSpace("N/A"),
                        INSubAgencyName = student.InternshipSubAgencyName.IfNullOrWhiteSpace("N/A"),
                        INEOD = student.InternshipEOD.HasValue ? student.InternshipEOD.Value.ToShortDateString() : "N/A",
                        AddINAgencyType = student.AdditionalInternshipAgencyType.IfNullOrWhiteSpace("N/A"),
                        AddINHDQAgencyName = student.AdditionalInternshipAgencyName.IfNullOrWhiteSpace("N/A"),
                        AddINSubAgencyName = student.AdditionalInternshipSubAgencyName.IfNullOrWhiteSpace("N/A"),
                        AddINReportedWebsite = student.AdditionalInternshipReportedWebsite.IfNullOrWhiteSpace("No"),
                        PGAgencyType = student.PostGradAgencyType.IfNullOrWhiteSpace("N/A"),
                        PGHDQAgencyName = student.PostGradAgencyName.IfNullOrWhiteSpace("N/A"),
                        PGSubAgencyName = student.PostGradSubAgencyName.IfNullOrWhiteSpace("N/A"),
                        AddPGAgencyType = student.AdditionalPostGradAgencyType.IfNullOrWhiteSpace("N/A"),
                        AddPGHDQAgencyName = student.AdditionalPostGradAgencyName.IfNullOrWhiteSpace("N/A"),
                        AddPGSubAgencyName = student.AdditionalPostGradSubAgencyName.IfNullOrWhiteSpace("N/A"),
                        AddPGReportedWebsite = student.AdditionalPostGradReportedWebsite.IfNullOrWhiteSpace("No"),
                        PGEOD = student.PostGradEOD.HasValue ? student.PostGradEOD.Value.ToShortDateString() : "N/A",
                        PGDaysBetween = "TBD",
                        DateLeftPGEarly = student.DateLeftPGEarly.Length > 0 ? Convert.ToDateTime(student.DateLeftPGEarly).ToShortDateString().ToString() : "N/A",
                        Citizenship = student.Citizenship.IfNullOrWhiteSpace("N/A"),
                        FollowupAction = student.FollowUpAction.IfNullOrWhiteSpace("N/A"),
                        FollowupActionType = student.FollowUpType.IfNullOrWhiteSpace("N/A"),
                        FollowupDate = student.FollowUpDate.IfNullOrWhiteSpace("N/A"),
                        PGVerificationOneDue = student.PGVerificationOneDueDate.IfNullOrWhiteSpace("N/A"),
                        PGVerificationOneComplete = student.PGVerificationOneCompleteDate.IfNullOrWhiteSpace("N/A"),
                        PGVerificationTwoDue = student.PGVerificationTwoDueDate.IfNullOrWhiteSpace("N/A"),
                        PGVerificationTwoComplete = student.PGVerificationTwoCompleteDate.IfNullOrWhiteSpace("N/A"),
                        CommitmentPhaseComplete = student.CommitmentPhaseComplete.IfNullOrWhiteSpace("N/A"),
                        ReleasePackageDueDate = student.DatePendingReleaseCollectionInfo.IfNullOrWhiteSpace("N/A"),
                        ReleasePackageSent = student.DateReleasedCollectionPackage.IfNullOrWhiteSpace("N/A"),
                        Amount = student.FundingAmount.IfNullOrWhiteSpace("N/A"),
                        TotalAcademicTerms = student.TotalAcademicTerms.IfNullOrWhiteSpace("N/A"),
                        ExtensionTypes = student.ExtensionType,
                        PGEmploymentDueDate = student.PGEmploymentDueDate.HasValue ? $"{student.PGEmploymentDueDate.Value.ToString("MMM")} {student.PGEmploymentDueDate.Value.Year.ToString()}" : "N/A",
                        PGPlacementCategory = student.PlacementCategory.IfNullOrWhiteSpace("N/A"),
                        RegistrationCode = (student.ProfileStatus.Equals("Not Registered") && await _featuremanager.IsEnabledSiteWideAsync("UnregisteredStudentFlow")) ? await _dashboardService.GenerateRegistrationCode(student.StudentUID.ToString()) : "N/A",
                        ProfileStatus = student.ProfileStatus,
                        SOCVerificationComplete = student.SOCVerificationComplete.IfNullOrWhiteSpace("N/A"),

                    });
                }

                return model;
            }
        }

        public class ReferenceDataViewModel
        {
            public List<string> Institutions { get; set; }
            public List<string> Degrees { get; set; }
            public List<string> Status { get; set; }
            public List<string> Majors { get; set; }
            public List<string> StatusOptions { get; set; }
            public List<string> Citizenship { get; set; }
            public List<string> Contract { get; set; }
            public List<string> FollowUpType { get; set; }
            public List<string> ExtensionTypes { get; set; }
            public List<int> Years { get; set; }
            public List<string> Sessions { get; set; }
            public Dictionary<string, List<string>> StatusOptionLookup { get; set; }
            public bool IsEnabledOnSite { get; set; }


        }
    }
}
