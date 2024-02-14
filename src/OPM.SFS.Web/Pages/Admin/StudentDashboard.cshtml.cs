using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Data.Migrations;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Models.Academia;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OPM.SFS.Web.SharedCode.CommitmentProcessService;
using System.Text;
using System.Security.Claims;
using AutoMapper.Internal;
using OPM.SFS.Core.DTO;
using OPM.SFS.Web.Models.Admin;
using static OPM.SFS.Web.Pages.Admin.DataManagementModel;
using Microsoft.Extensions.Configuration;
using static OPM.SFS.Web.Pages.Student.RegistrationDetailsModel;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OPM.SFS.Web.SharedCode.Repositories;
using Microsoft.ApplicationInsights.WindowsServer;
using OPM.SFS.Web.SharedCode.StudentDashboardRules;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    [IgnoreAntiforgeryToken] //THIS IS JUST FOR TESTING!! NEED TO FIX ASAP!!!!

    //Add cell validations - https://blog.ag-grid.com/user-input-validation-with-ag-grid/
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
            var data = _mediator.Send(new JsonQuery()).Result;
            return new JsonResult(data);
        }

        public async Task<JsonResult> OnGetAllReferenceData()
        {
            Data = await _mediator.Send(new Query() { });
            return new JsonResult(Data);
        }

        public JsonResult OnPostInsertNewStudent([FromBody] AdminNewStudentVM model)
        {
            var data = _mediator.Send(new JsonInsertNewStudent() { Model = model }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnPostInsertStudentFunding([FromBody] AdminNewStudentVM model)
        {
            var data = _mediator.Send(new JsonInsertStudentFunding() { Model = model }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnPostUpdateStudent([FromBody] StudentDashboardViewModel model)
        {
            if (User.FindFirst("AdminRole").Value != "Read Only")
            {
                var data = _mediator.Send(new JsonUpdateStudent() { Model = model, AdminUserID = Convert.ToInt32(User.FindFirst("SFS_UserID").Value) }).Result;
                return new JsonResult(data);
            }
            else
            {
                return new JsonResult(model);
            }
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

                foreach(var option in statusOptionKeys)
                {
                    var statusByOption = statusoption.Where(m => m.Option == option && m.IsDeleted == false).Select(m => m.Status).ToList();
                    model.StatusOptionLookup.Add(option, statusByOption);
                }
                return model;               

            }
        }

        public class JsonInsertNewStudent : IRequest<JsonInsertStudentHandler.JsonInsertNewStudentResult>
        {
            public AdminNewStudentVM Model { get; set; }
        }

        public class JsonInsertStudentHandler : IRequestHandler<JsonInsertNewStudent, JsonInsertStudentHandler.JsonInsertNewStudentResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICryptoHelper _crypto;
            private readonly IEmailerService _emailer;
            private readonly IReferenceDataRepository _refRepo;
            private readonly IConfiguration _config;
            private readonly IStudentDashboardService _dashboardService;
            private readonly IFeatureManager _featureManager;


			public JsonInsertStudentHandler(ScholarshipForServiceContext db, ICryptoHelper crypto , IEmailerService emailer, IReferenceDataRepository refRepo, IConfiguration config, IStudentDashboardService dashService, IFeatureManager featureManager)
            {
                _db = db;
                _crypto = crypto;
                _emailer = emailer;
                _refRepo = refRepo;
                _config = config;
                _dashboardService = dashService;
                _featureManager = featureManager;
            }
            public async Task<JsonInsertNewStudentResult> Handle(JsonInsertNewStudent request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
                var lastUID = await _db.Students.MaxAsync(m => m.StudentUID);
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                var allStatus = await _refRepo.GetProfileStatusAsync();
                var citizenships = await _refRepo.GetCitizenshipAsync();
                var institutions = await _refRepo.GetInstitutionsAsync();
                var majors = await _refRepo.GetDisciplinesAsync();
                var degrees = await _refRepo.GetDegreesAsync();
                var contracts = await _refRepo.GetContractAsync();
                
                var NotRegisteredID = allStatus.Where(m => m.Name == "Not Registered").Select(m => m.ProfileStatusID).FirstOrDefault();
				string baseUrl = _config["General:BaseUrl"];

                var existingName = await _db.Students.Where(m => m.FirstName.ToLower() == request.Model.FirstName.Trim().ToLower() && m.LastName.ToLower() == request.Model.LastName.Trim().ToLower())
                .FirstOrDefaultAsync();
                if (existingName != null)
                {
                    return new JsonInsertNewStudentResult() { IsSuccessful = false, ErrorMessage = "This is a duplicate student based on name.", IsDuplicate = true, ID = existingName.StudentUID };
                }

                var existingEmail = await _db.StudentAccount.Where(m => m.UserName == request.Model.Email).FirstOrDefaultAsync();
                if (existingEmail != null)
                    return new JsonInsertNewStudentResult() { IsSuccessful = false, ErrorMessage = "An account with this email already exists." };



                SFS.Data.Student newStudent = new();
                newStudent.StudentInstitutionFundings = new List<StudentInstitutionFunding>();
                var institutionInfo = institutions.Where(m => m.Name.TrimEnd() == request.Model.Institution).FirstOrDefault();
                var totalTerms = _dashboardService.CalculateTotalTerms(request.Model.EnrolledYear, request.Model.EnrolledSession, request.Model.FundingEndYear, request.Model.FundingEndSession, institutionInfo.AcademicSchedule.Name);
                var pgEmployDueDate = _dashboardService.CalculatePGEmploymentDate(Convert.ToDateTime(request.Model.ExpectedGradDate), 18, 0);
                var serviceOwedLookup = _dashboardService.GetServiceOwed(institutionInfo.AcademicSchedule.Name, totalTerms, request.Model.EnrolledYear, request.Model.EnrolledSession, request.Model.FundingEndYear, request.Model.FundingEndSession);
                double serviceOwed;
                newStudent.FirstName = textinfo.ToTitleCase(request.Model.FirstName.ToLower());
                newStudent.LastName = textinfo.ToTitleCase(request.Model.LastName.ToLower());
                newStudent.Email = request.Model.Email;
                newStudent.DateAdded = DateTime.UtcNow;
                newStudent.ProfileStatusID = NotRegisteredID;
                newStudent.StudentUID = lastUID + 1;
                newStudent.CitizenshipID = citizenships.Where(m => m.Value == request.Model.Citizenship).Select(m => m.CitizenshipID).FirstOrDefault();
                newStudent.StudentAccount = new StudentAccount()
                {
                    UserName = request.Model.Email,
                    Password = "",
                    IsDisabled = false,
                    FailedLoginCount = 0,
                    LastLoginDate = null
                };

                newStudent.StudentInstitutionFundings.Add(new StudentInstitutionFunding()
                {
                    InstitutionId = institutionInfo.InstitutionId,
                    MajorId = majors.Where(m => m.Name == request.Model.Major).Select(m => m.DisciplineId).FirstOrDefault(),
                    DegreeId = degrees.Where(m => m.Name == request.Model.Degree).Select(m => m.DegreeId).FirstOrDefault(),
                    ExpectedGradDate = Convert.ToDateTime(request.Model.ExpectedGradDate),
                    EnrolledSession = request.Model.EnrolledSession,
                    EnrolledYear = Convert.ToInt32(request.Model.EnrolledYear),
                    FundingEndSession = request.Model.FundingEndSession,
                    FundingEndYear = Convert.ToInt32(request.Model.FundingEndYear),
                    MinorId = !request.Model.Minor.Equals("Please select") ? majors.Where(m => m.Name == request.Model.Minor).Select(m => m.DisciplineId).FirstOrDefault() : null,
                    ContractId = contracts.Where(m => m.Name == request.Model.Contract).Select(m => m.ContractId).FirstOrDefault(),
                    TotalAcademicTerms = totalTerms,
                    PGEmploymentDueDate = !string.IsNullOrWhiteSpace(pgEmployDueDate) && pgEmployDueDate != "N/A" ? Convert.ToDateTime(pgEmployDueDate) : null,
                    ServiceOwed = double.TryParse(serviceOwedLookup, out serviceOwed) ? serviceOwed : null,
                    SecondDegreeMajorId = !request.Model.SecondMajor.Equals("Please select") ? majors.Where(m => m.Name == request.Model.SecondMajor).Select(m => m.DisciplineId).FirstOrDefault() : null,
                    SecondDegreeMinorId = !request.Model.SecondMinor.Equals("Please select") ? majors.Where(m => m.Name == request.Model.SecondMinor).Select(m => m.DisciplineId).FirstOrDefault() : null

                }) ;
                if (await _featureManager.IsEnabledSiteWideAsync("UnregisteredStudentFlow") )
                {
                    EmailTemplateModel emailData = new()
                    {
                        StudentFullName = $"{newStudent.FirstName}",
                        RegistrationCode = $"{await _dashboardService.GenerateRegistrationCode(newStudent.StudentUID.ToString())}",
                        StudentInstitution = institutionInfo.Name,
                        BaseURL = baseUrl,
					};

                    await _emailer.SendEmailWithTemplateAsync(newStudent.Email, "To_ST_Unregistered_Student_Upload", emailData);
                }
                await _db.Students.AddAsync(newStudent);
                await _db.SaveChangesAsync();

                return new JsonInsertNewStudentResult() { IsSuccessful = true, ErrorMessage = "" };
            }

            
            public class JsonInsertNewStudentResult
            {
                public bool IsSuccessful { get; set; }
                public string ErrorMessage { get; set; }
                public bool IsDuplicate { get; set; }
                public int ID { get; set; }
            }
        }

        public class JsonInsertStudentFunding : IRequest<JsonInsertStudentHandler.JsonInsertNewStudentResult>
        {
            public AdminNewStudentVM Model { get; set; }
        }

        public class JsonInsertStudentFundingHandler : IRequestHandler<JsonInsertStudentFunding, JsonInsertStudentHandler.JsonInsertNewStudentResult>
        {

            private readonly ScholarshipForServiceContext _db;
            private readonly IReferenceDataRepository _refRepo;
            private readonly IStudentDashboardService _dashboardService;
            public JsonInsertStudentFundingHandler(ScholarshipForServiceContext db, IReferenceDataRepository refRepo, IStudentDashboardService dashboardService)
            {
                _db= db;
                _refRepo= refRepo;
                _dashboardService= dashboardService;
            }
            public async Task<JsonInsertStudentHandler.JsonInsertNewStudentResult> Handle(JsonInsertStudentFunding request, CancellationToken cancellationToken)
            {               
                var institutions = await _refRepo.GetInstitutionsAsync();
                var majors = await _refRepo.GetDisciplinesAsync();
                var degrees = await _refRepo.GetDegreesAsync();
                var contracts = await _refRepo.GetContractAsync();
                var student = await _db.Students.Where(m => m.FirstName.ToLower() == request.Model.FirstName.Trim().ToLower() && m.LastName.ToLower() == request.Model.LastName.Trim().ToLower())
                   .FirstOrDefaultAsync();
                var institutionInfo = institutions.Where(m => m.Name.TrimEnd() == request.Model.Institution).FirstOrDefault();
                var totalTerms = _dashboardService.CalculateTotalTerms(request.Model.EnrolledYear, request.Model.EnrolledSession, request.Model.FundingEndYear, request.Model.FundingEndSession, institutionInfo.AcademicSchedule.Name);
                var pgEmployDueDate = _dashboardService.CalculatePGEmploymentDate(Convert.ToDateTime(request.Model.ExpectedGradDate), 18, 0);
                var serviceOwedLookup = _dashboardService.GetServiceOwed(institutionInfo.AcademicSchedule.Name, totalTerms, request.Model.EnrolledYear, request.Model.EnrolledSession, request.Model.FundingEndYear, request.Model.FundingEndSession);
                double serviceOwed;
                student.StudentInstitutionFundings.Add(new StudentInstitutionFunding()
                {                   
                    InstitutionId = institutionInfo.InstitutionId,
                    MajorId = majors.Where(m => m.Name == request.Model.Major).Select(m => m.DisciplineId).FirstOrDefault(),
                    DegreeId = degrees.Where(m => m.Name == request.Model.Degree).Select(m => m.DegreeId).FirstOrDefault(),
                    ExpectedGradDate = Convert.ToDateTime(request.Model.ExpectedGradDate),
                    EnrolledSession = request.Model.EnrolledSession,
                    EnrolledYear = Convert.ToInt32(request.Model.EnrolledYear),
                    FundingEndSession = request.Model.FundingEndSession,
                    FundingEndYear = Convert.ToInt32(request.Model.FundingEndYear),
                    MinorId = !request.Model.Minor.Equals("Please select") ? majors.Where(m => m.Name == request.Model.Minor).Select(m => m.DisciplineId).FirstOrDefault() : null,
                    ContractId = contracts.Where(m => m.Name == request.Model.Contract).Select(m => m.ContractId).FirstOrDefault(),
                    TotalAcademicTerms = totalTerms,
                    PGEmploymentDueDate = !string.IsNullOrWhiteSpace(pgEmployDueDate) && pgEmployDueDate != "N/A" ? Convert.ToDateTime(pgEmployDueDate) : null,
                    ServiceOwed = double.TryParse(serviceOwedLookup, out serviceOwed) ? serviceOwed : null,
                    SecondDegreeMajorId = !request.Model.SecondMajor.Equals("Please select") ? majors.Where(m => m.Name == request.Model.SecondMajor).Select(m => m.DisciplineId).FirstOrDefault() : null,
                    SecondDegreeMinorId = !request.Model.SecondMinor.Equals("Please select") ? majors.Where(m => m.Name == request.Model.SecondMinor).Select(m => m.DisciplineId).FirstOrDefault() : null
                });
                await _db.SaveChangesAsync();
                return new JsonInsertStudentHandler.JsonInsertNewStudentResult() { IsSuccessful= true };
            }
        }

        public class JsonQuery : IRequest<IEnumerable<StudentDashboardViewModel>>
        {

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
                
                //var studentData = await _repo.GetAllStudentsForDashboardAsync();
                var studentData = await _repo.GetAllStudentForDashboardWithSP();
                List<StudentDashboardViewModel> model = new();

                foreach (var student in studentData)
                {
                    StudentDashboardViewModel vm = new StudentDashboardViewModel();
                    vm.ID = student.StudentUID.ToString();
                    vm.StudentID = student.StudentID.ToString();
                    vm.FundingID = student.StudentInstitutionFundingId.ToString();
                    vm.Studentname = $"{student.Firstname} {student.LastName}";
                    vm.EditUrl = $"/admin/StudentProfileEdit?sid={student.StudentID}";
                    vm.Institution = student.Institution;
                    vm.InstitutionID = student.InstitutionID.ToString();
                    vm.SchoolType = student.SchoolType;
                    vm.EnrolledSession = student.EnrolledSession.IfNullOrWhiteSpace("N/A");
                    vm.EnrolledYear = student.EnrolledYear.IfNullOrWhiteSpace("N/A");
                    vm.Registered = student.ProfileStatus.Equals("Not Registered") ? "N" : "Y";
                    vm.FundingEndSession = student.FundingEndSession.IfNullOrWhiteSpace("N/A");
                    vm.FundingEndYear = student.FundingEndYear.IfNullOrWhiteSpace("N/A");
					vm.GraduationDateMonth = !string.IsNullOrWhiteSpace(student.GraduationDate) ? Convert.ToDateTime(student.GraduationDate).ToString("MMM") : "N/A";
					vm.GraduationDateYear = !string.IsNullOrWhiteSpace(student.GraduationDate) ? Convert.ToDateTime(student.GraduationDate).Year.ToString() : "N/A";
					vm.StatusOption = string.IsNullOrWhiteSpace(student.StatusOption) ? "N/A" : student.StatusOption;
                    vm.ProgramPhase = string.IsNullOrWhiteSpace(student.ProgramPhase) ? "N/A" : student.ProgramPhase;
                    vm.EmailAddress = student.EmailAddress;
                    vm.Degree = student.Degree;
                    vm.AltEmail = student.AltEmail.IfNullOrWhiteSpace("N/A");
                    vm.Major = student.Major;
                    vm.InstitutionGroup = "TBD";
                    vm.Minor = student.Minor.IfNullOrWhiteSpace("N/A");
                    vm.SecondDegreeMajor = student.SecondDegreeMajor.IfNullOrWhiteSpace("N/A");
                    vm.SecondDegreeMinor = student.SecondDegreeMinor.IfNullOrWhiteSpace("N/A");
                    vm.AcademicSchedule = student.AcademicSchedule;
                    vm.Status = student.Status.IfNullOrWhiteSpace("N/A");
                    vm.ServiceOwed = student.ServiceOwed.HasValue ? student.ServiceOwed.Value.ToString() : "N/A";
                    vm.Contract = student.Contract.IfNullOrWhiteSpace("N/A");
                    vm.DeferralAgreementReceived = "TBD";
                    vm.INReported = student.InternshipReported.IfNullOrWhiteSpace("No");
                    vm.PGReported = student.PostGradReported.IfNullOrWhiteSpace("No");
                    vm.INAgencyType = student.InternshipAgencyType.IfNullOrWhiteSpace("N/A");
                    vm.INHDQAgencyName = student.InternshipAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.INSubAgencyName = student.InternshipSubAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.INEOD = student.InternshipEOD.HasValue ? student.InternshipEOD.Value.ToShortDateString() : "N/A";
                    vm.AddINAgencyType = student.AdditionalInternshipAgencyType.IfNullOrWhiteSpace("N/A");
                    vm.AddINHDQAgencyName = student.AdditionalInternshipAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.AddINSubAgencyName = student.AdditionalInternshipSubAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.AddINReportedWebsite = student.AdditionalInternshipReportedWebsite.IfNullOrWhiteSpace("No");
                    vm.PGAgencyType = student.PostGradAgencyType.IfNullOrWhiteSpace("N/A");
                    vm.PGHDQAgencyName = student.PostGradAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.PGSubAgencyName = student.PostGradSubAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.AddPGAgencyType = student.AdditionalPostGradAgencyType.IfNullOrWhiteSpace("N/A");
                    vm.AddPGHDQAgencyName = student.AdditionalPostGradAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.AddPGSubAgencyName = student.AdditionalPostGradSubAgencyName.IfNullOrWhiteSpace("N/A");
                    vm.AddPGReportedWebsite = student.AdditionalPostGradReportedWebsite.IfNullOrWhiteSpace("No");
                    vm.PGEOD = student.PostGradEOD.HasValue ? student.PostGradEOD.Value.ToShortDateString() : "N/A";
                    vm.PGDaysBetween = "TBD";
                    vm.DateLeftPGEarly = !string.IsNullOrWhiteSpace(student.DateLeftPGEarly) ? Convert.ToDateTime(student.DateLeftPGEarly).ToShortDateString().ToString() : "N/A";
                    vm.Citizenship = student.Citizenship.IfNullOrWhiteSpace("N/A");
                    vm.LastUpdateReceived = student.LastUpdated.HasValue ? student.LastUpdated.Value.ToShortDateString() : "N/A";
                    vm.FollowupAction = student.FollowUpAction.IfNullOrWhiteSpace("N/A");
                    vm.FollowupActionType = student.FollowUpType.IfNullOrWhiteSpace("N/A");
                    vm.FollowupDate = student.FollowUpDate.IfNullOrWhiteSpace("N/A");
                    vm.PGVerificationOneDue = student.PGVerificationOneDueDate.IfNullOrWhiteSpace("N/A");
                    vm.PGVerificationOneComplete = student.PGVerificationOneCompleteDate.IfNullOrWhiteSpace("N/A");
                    vm.PGVerificationTwoDue = student.PGVerificationTwoDueDate.IfNullOrWhiteSpace("N/A");
                    vm.PGVerificationTwoComplete = student.PGVerificationTwoCompleteDate.IfNullOrWhiteSpace("N/A");
                    vm.CommitmentPhaseComplete = student.CommitmentPhaseComplete.IfNullOrWhiteSpace("N/A");
                    vm.ReleasePackageDueDate = student.DatePendingReleaseCollectionInfo.IfNullOrWhiteSpace("N/A");
                    vm.ReleasePackageSent = student.DateReleasedCollectionPackage.IfNullOrWhiteSpace("N/A");
                    vm.Amount = student.FundingAmount.IfNullOrWhiteSpace("N/A");
                    vm.TotalAcademicTerms = student.TotalAcademicTerms.IfNullOrWhiteSpace("N/A");
                    vm.ExtensionTypes = student.ExtensionType.IfNullOrWhiteSpace("No Extension");
                    vm.PGEmploymentDueDate = student.PGEmploymentDueDate.HasValue ? $"{student.PGEmploymentDueDate.Value.ToString("MMM")} {student.PGEmploymentDueDate.Value.Year.ToString()}" : "N/A";
                    vm.StudentNote = student.Notes.IfNullOrWhiteSpace("N/A");
                    vm.PGPlacementCategory = student.PlacementCategory.IfNullOrWhiteSpace("N/A");
                    vm.RegistrationCode = (student.ProfileStatus.Equals("Not Registered") && await _featuremanager.IsEnabledSiteWideAsync("UnregisteredStudentFlow")) ? await _dashboardService.GenerateRegistrationCode(student.StudentUID.ToString()) : "N/A";
                    vm.ProfileStatus = student.ProfileStatus;
                    vm.SOCVerificationComplete = student.SOCVerificationComplete.IfNullOrWhiteSpace("N/A");
                    model.Add(vm);
                }

                return model;
            }
        }

        public class JsonUpdateStudent : IRequest<StudentDashboardViewModel>
        {
            public StudentDashboardViewModel Model { get; set; }
            public int AdminUserID { get; set; }
        }

        public class JsonUpdateStudentHandler : IRequestHandler<JsonUpdateStudent, StudentDashboardViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;            
            private readonly IStudentDashboardService _dashboardService;
			private readonly IReferenceDataRepository _repo;
            private readonly IStudentDashboardRepository _dashRepo;
            private readonly IStudentDashboardRulesEngine _rulesEngine;



			public JsonUpdateStudentHandler(ScholarshipForServiceContext efDB, IStudentDashboardService dashService, IReferenceDataRepository repo, IStudentDashboardRepository dashrepo, IStudentDashboardRulesEngine rulesEngine)
            {
                _efDB = efDB;                
                _dashboardService = dashService;
                _repo = repo;
                _dashRepo = dashrepo;
                _rulesEngine= rulesEngine;

            }

            public async Task<StudentDashboardViewModel> Handle(JsonUpdateStudent request, CancellationToken cancellationToken)
            {
				var statusoption = await _repo.GetStatusOptionsAsync();
				var fundingRecordToUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == Convert.ToInt32(request.Model.StudentID) && m.StudentInstitutionFundingId == Convert.ToInt32(request.Model.FundingID)).FirstOrDefaultAsync();
				var studentRecordToUpdate = await _efDB.Students.Where(m => m.StudentUID == Convert.ToInt32(request.Model.ID)).FirstOrDefaultAsync();
				await _rulesEngine.BuildUpdateAsync(fundingRecordToUpdate, studentRecordToUpdate, request.Model);               
                var dataBeforeLog = FormatDataForLogging(fundingRecordToUpdate, studentRecordToUpdate);

                await _efDB.SaveChangesAsync();
                var dataAfterLog = FormatDataForLogging(fundingRecordToUpdate, studentRecordToUpdate);
                await _dashRepo.InsertStudentDashboardLog(dataBeforeLog, dataAfterLog, request.AdminUserID);

                StudentDashboardViewModel vm = new();
                vm.ID = studentRecordToUpdate.StudentUID.ToString();
                vm.EnrolledSession = fundingRecordToUpdate.EnrolledSession;
                vm.EnrolledYear = fundingRecordToUpdate.EnrolledYear.ToString();
                vm.FundingEndSession = fundingRecordToUpdate.FundingEndSession;
                vm.FundingEndYear = fundingRecordToUpdate.FundingEndYear.ToString();
                vm.Contract = fundingRecordToUpdate.Contract != null ? fundingRecordToUpdate.Contract.Name : "";
                vm.PGPlacementCategory = statusoption.Where(m => m.Status == request.Model.Status).Select(m => m.PostGradPlacementGroup).FirstOrDefault(); 
                vm.ProgramPhase = statusoption.Where(m => m.Status == request.Model.Status).Select(m => m.Phase).FirstOrDefault(); ;
				vm.FollowupAction = fundingRecordToUpdate.FollowUpAction.IfNullOrWhiteSpace("");
                vm.FollowupDate = fundingRecordToUpdate.FollowUpDate.HasValue ? fundingRecordToUpdate.FollowUpDate.Value.ToShortDateString() : "N/A";
                vm.FollowupActionType = fundingRecordToUpdate.FollowUpTypeOption != null ? fundingRecordToUpdate.FollowUpTypeOption.Name : "N/A";              
                vm.TotalAcademicTerms = fundingRecordToUpdate.TotalAcademicTerms;                
                if (fundingRecordToUpdate.PGEmploymentDueDate.HasValue)
                {
                    string formatDate = $"{fundingRecordToUpdate.PGEmploymentDueDate.Value.ToString("MMM")} {fundingRecordToUpdate.PGEmploymentDueDate.Value.Year.ToString()}";
                    vm.PGEmploymentDueDate = formatDate;
                }
                vm.ServiceOwed = fundingRecordToUpdate.ServiceOwed.HasValue ? fundingRecordToUpdate.ServiceOwed.Value.ToString() : "N/A";
                vm.DateLeftPGEarly = fundingRecordToUpdate.DateLeftPGEarly.HasValue ? fundingRecordToUpdate.DateLeftPGEarly.Value.ToShortDateString() : "N/A";
                vm.PGVerificationOneDue = fundingRecordToUpdate.PGVerificationOneDueDate.HasValue ? fundingRecordToUpdate.PGVerificationOneDueDate.Value.ToShortDateString() : "N/A";
                vm.PGVerificationTwoDue = fundingRecordToUpdate.PGVerificationTwoDueDate.HasValue ? fundingRecordToUpdate.PGVerificationTwoDueDate.Value.ToShortDateString() : "N/A";
                vm.CommitmentPhaseComplete = fundingRecordToUpdate.CommitmentPhaseComplete.HasValue ? fundingRecordToUpdate.CommitmentPhaseComplete.Value.ToShortDateString() : "N/A";
                vm.SOCVerificationComplete = fundingRecordToUpdate.SOCVerificationComplete.HasValue ? fundingRecordToUpdate.SOCVerificationComplete.Value.ToShortDateString() : "N/A";
                vm.ReleasePackageDueDate = fundingRecordToUpdate.DatePendingReleaseCollectionInfo.ToString().IfNullOrWhiteSpace("N/A");
                vm.ReleasePackageSent = fundingRecordToUpdate.DateReleasedCollectionPackage.ToString().IfNullOrWhiteSpace("N/A");
                vm.Citizenship = studentRecordToUpdate.Citizenship != null ? studentRecordToUpdate.Citizenship.Value : "N/A";
                return vm;

			}

            private StudentDashboardLogDTO FormatDataForLogging(OPM.SFS.Data.StudentInstitutionFunding studentFundingInfo, OPM.SFS.Data.Student studentInfo)
            {
                StudentDashboardLogDTO logData = new StudentDashboardLogDTO();
                logData.StudentID = studentFundingInfo.StudentId;
                logData.StudentUID = studentInfo.StudentUID;
                logData.Contract = studentFundingInfo.ContractId.HasValue ? studentFundingInfo.ContractId.Value.ToString() : "null";
                logData.TotalTerms = studentFundingInfo.TotalAcademicTerms;
                logData.PGExtensionType = studentFundingInfo.ExtensionTypeID.HasValue ? studentFundingInfo.ExtensionTypeID.Value.ToString() : "null";
                logData.PGEmploymentDueDate = studentFundingInfo.PGEmploymentDueDate.HasValue ? studentFundingInfo.PGEmploymentDueDate.Value.ToShortDateString() : "null";
                logData.DateLeftPGEarly = studentFundingInfo.TotalAcademicTerms;
                logData.Status = studentFundingInfo.StatusID.HasValue ? studentFundingInfo.StatusID.Value.ToString() : "null";
                logData.PgVerificationOneDue = studentFundingInfo.PGVerificationOneDueDate.HasValue ? studentFundingInfo.PGVerificationOneDueDate.Value.ToShortDateString() : "null";
                logData.PgVerificationOneComplete = studentFundingInfo.PGVerificationOneCompleteDate.HasValue ? studentFundingInfo.PGVerificationOneCompleteDate.Value.ToShortDateString() : "null"; 
                logData.PgVerificationTwoDue = studentFundingInfo.PGVerificationTwoDueDate.HasValue ? studentFundingInfo.PGVerificationTwoDueDate.Value.ToShortDateString() : "null";
                logData.PgVerificationTwoComplete = studentFundingInfo.PGVerificationTwoCompleteDate.HasValue ? studentFundingInfo.PGVerificationTwoCompleteDate.Value.ToShortDateString() : "null";
                logData.CommitmentPhaseComplete = studentFundingInfo.CommitmentPhaseComplete.HasValue ? studentFundingInfo.CommitmentPhaseComplete.Value.ToShortDateString() : "null";
				logData.SOCVerificationComplete = studentFundingInfo.SOCVerificationComplete.HasValue ? studentFundingInfo.SOCVerificationComplete.Value.ToShortDateString() : "null";
				logData.Note = studentFundingInfo.Notes;
                logData.FollowupDate = studentFundingInfo.FollowUpDate.HasValue ? studentFundingInfo.FollowUpDate.Value.ToShortDateString() : "null";
                logData.FollowupAction = studentFundingInfo.FollowUpAction;
                logData.ReleasePackageDueDate = studentFundingInfo.DatePendingReleaseCollectionInfo.HasValue ? studentFundingInfo.DatePendingReleaseCollectionInfo.Value.ToShortDateString() : "null";
                logData.ReleasePackageSent = studentFundingInfo.DateReleasedCollectionPackage.HasValue ? studentFundingInfo.DateReleasedCollectionPackage.Value.ToShortDateString() : "null";
                logData.Amount = studentFundingInfo.FundingAmount.HasValue ? studentFundingInfo.FundingAmount.Value.ToString() : "null";
                logData.Citizenship = studentInfo.CitizenshipID.HasValue ? studentInfo.CitizenshipID.Value.ToString() : "null";
                return logData;
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
