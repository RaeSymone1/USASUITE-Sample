using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Models.Student;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Student
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class RegistrationDetailsModel : PageModel
    {   
      
       
        private readonly IMediator _mediator;
        private readonly ICryptoHelper _crypto;
        private readonly ICacheHelper _cache;
        private readonly ILogger<RegistrationDetailsModel> _logger;
        private readonly IStudentRepository _repo;
        private readonly IFeatureManager _featureManager;

        public RegistrationDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public StudentRegistrationViewModel Data { get; set; }
        [FromQuery(Name = "uid")]
        public string AccountID { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {               
                if (AccountID.Length < 1)                
                    return RedirectToPage("/Student/Registration");
                
                Data = await _mediator.Send(new Query() { Id = AccountID });
                if(!Data.IsValidRegistrationAttempt)
                    return RedirectToPage("/Student/Registration");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration code decrypt error for {AccountID}");
                _logger.LogError($"Error message {ex.Message}. Stack {ex.StackTrace}");
            }
            return RedirectToPage("/Student/Registration");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                //re-populate dropdown lists on errors
                Data = await _mediator.Send(new Query());
                return Page();
            }
                
            var result = await _mediator.Send(new Command() { Model = Data});
            if (result.IsSuccessful) return RedirectToPage("/Student/RegistrationConfirm");
            Data = await _mediator.Send(new Query() {  Id = result.EncryptedUID});
            ModelState.AddModelError("", result.ErrorMessage);
            return Page();
        }

        public class Query : IRequest<StudentRegistrationViewModel>
        {
            public string Id { get; set; }
        }
        

        public class QueryHandler : IRequestHandler<Query, StudentRegistrationViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly IReferenceDataRepository _refRepo;
            private readonly IFeatureManager _featureManager;
            private readonly ICryptoHelper _crypto;

            public QueryHandler(ScholarshipForServiceContext db, IReferenceDataRepository refRepo, IFeatureManager featureManager, ICryptoHelper crypto)
            {
                _db = db;
                _refRepo = refRepo;
                _featureManager = featureManager;
                _crypto = crypto;
            }

            public async Task<StudentRegistrationViewModel> Handle(Query request, CancellationToken cancellationToken)
            {

                StudentRegistrationViewModel model = new StudentRegistrationViewModel();
               
                var institutions = await _refRepo.GetInstitutionsAsync();
                model.Institutions = new SelectList(institutions.OrderBy(m => m.Name), nameof(Institution.InstitutionId), nameof(Institution.Name));
                model.Degrees = new SelectList(await _refRepo.GetDegreesAsync(), nameof(Degree.DegreeId), nameof(Degree.Name));
                model.Disciplines  = new SelectList(await DisciplineSortForNAAsync(), nameof(Discipline.DisciplineId), nameof(Discipline.Name));
                model.IsValidRegistrationAttempt = true;
                if (await _featureManager.IsEnabledSiteWideAsync("UnregisteredStudentFlow"))
                {
                    var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();

                    var _uid = Convert.ToInt64(_crypto.Decrypt(request.Id, GlobalConfigSettings));
                    
                    var data = await _db.Students.AsNoTracking()
                     .Where(m => m.StudentUID == _uid)
                    .Select(m => new
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        GradDate = m.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate,
                        Email = m.Email,
                        Degree = m.StudentInstitutionFundings.FirstOrDefault().DegreeId,
                        Major = m.StudentInstitutionFundings.FirstOrDefault().MajorId,
                        College = m.StudentInstitutionFundings.FirstOrDefault().InstitutionId,
                        Minor = m.StudentInstitutionFundings.FirstOrDefault().MinorId,
                        SecondMajor = m.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajorId,
                        SecondMinor = m.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinorId,
                        ProfileStatus = m.ProfileStatus.Name
                    })
                     .FirstOrDefaultAsync();
                    
                    if(data.ProfileStatus != "Not Registered")
                    {
                        model.IsValidRegistrationAttempt = false;
                        return model;
                    } 

                    if (data != null)
                    {                        
                        model.Firstname = data.FirstName;
                        model.Lastname = data.LastName;
                        model.GraduationDate = data.GradDate.HasValue ? $"{data.GradDate.Value.Month.ToString("d2")}/{data.GradDate.Value.Year}" : "";
                        model.Email = data.Email;
                        model.SelectedDegree = data.Degree;
                        model.SelectedDiscipline = data.Major;
                        model.SelectedMinor = data.Minor;
                        model.SelectedSecondDegreeMajor = data.SecondMajor;
                        model.SelectedSecondDegreeMinor = data.SecondMinor;
                        model.SelectedCollege = data.College;
                        model.UID = _uid;
                    }
                }
                return model;
            }

            public async Task<List<Discipline>> DisciplineSortForNAAsync()
            {
                var disciplines = await _refRepo.GetDisciplinesAsync();
                disciplines = disciplines.OrderBy(m => m.Name).ToList();
                var naIndex = disciplines.FindIndex(x => x.Name == "N/A");
                var naValue = disciplines[naIndex];
                disciplines.Insert(0, naValue);
                disciplines.RemoveAt(naIndex);
                return disciplines;

            }
        }


        public class RegistrationDetailsValidator : AbstractValidator<StudentRegistrationViewModel>
        {
            public RegistrationDetailsValidator()
            {
                RuleFor(request => request.Firstname).NotEmpty();
                RuleFor(request => request.Firstname).Matches("^[^><&]+$");
                RuleFor(request => request.Lastname).NotEmpty();
                RuleFor(request => request.Lastname).Matches("^[^><&]+$");
                RuleFor(request => request.SSN).NotEmpty();
                RuleFor(request => request.ConfirmSSN).NotEmpty();
                RuleFor(request => request.SSN).Equal(request => request.ConfirmSSN);
                RuleFor(request => request.DateOfBirth).NotEmpty().Matches("^(1[0-2]|0?[1-9])/(3[01]|[12][0-9]|0?[1-9])/[0-9]{4}$")
                    .WithMessage("Date of birth is invalid date format."); 
                RuleFor(request => request.SelectedCollege).NotEmpty();
                RuleFor(request => request.SelectedDiscipline).NotEmpty();
                RuleFor(request => request.SelectedDegree).NotEmpty();
                RuleFor(request => request.InitialFundingDate).NotEmpty().Matches("(0[1-9]|10|11|12)/20[0-9]{2}$")
                    .WithMessage("Inital Funding date is invalid date format.");
                RuleFor(request => request.GraduationDate).NotEmpty().Matches("(0[1-9]|10|11|12)/20[0-9]{2}$")
                    .WithMessage("Graduation Date is invalid date format.");
                RuleFor(request => request.Email).NotEmpty().EmailAddress();
                RuleFor(request => request.AlternateEmail).NotEmpty().EmailAddress().WithMessage("Alternate email cannot be the same as the primary email.");
                RuleFor(request => request.Email).NotEqual(request => request.AlternateEmail);

               
            }
         
        }

        public class RegistrationCommandResponse 
        {
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
            public string EncryptedUID { get; set; }
        }

        public class Command : IRequest<RegistrationCommandResponse>
        {           
            public StudentRegistrationViewModel Model { get; set; }
            
        }

        public class CommandHandler : IRequestHandler<Command, RegistrationCommandResponse>
        {
            private readonly IReferenceDataRepository _refRepo;
            private readonly ICryptoHelper _crypto;
            private readonly IEmailerService _emailer;
            private readonly IStudentRepository _repo;
            private readonly IConfiguration _config;
            private readonly IFeatureManager _featureManager;


            public CommandHandler(IReferenceDataRepository refRepo, ICryptoHelper crypto, IEmailerService emailer, IStudentRepository repo, IConfiguration config, IFeatureManager featureManager)
            {
                _refRepo = refRepo;
                _crypto = crypto;
                _emailer = emailer;
                _repo = repo;
                _config = config;
                _featureManager = featureManager;
            }
            public async Task<RegistrationCommandResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
                string encryptedUID = _crypto.Encrypt(request.Model.UID.ToString(), GlobalConfigSettings);
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
               
                var degrees = await _refRepo.GetDegreesAsync();                
                var existingStudentByEmail = await _repo.GetStudentByEmail(request.Model.Email);
                if (existingStudentByEmail != null && existingStudentByEmail.ProfileStatus != "Not Registered")
                    return new RegistrationCommandResponse() { IsSuccessful = false, ErrorMessage = "An account with this email already exists.", EncryptedUID = encryptedUID };

                var existingStudentBySSN = await _repo.GetStudentBySSN(_crypto.Encrypt(request.Model.SSN, GlobalConfigSettings));
               if(existingStudentBySSN != null && existingStudentBySSN.ProfileStatus != "Not Registered")
                    return new RegistrationCommandResponse() { IsSuccessful = false, ErrorMessage = "An account with this SSN already exists.", EncryptedUID = encryptedUID };
                var degreeName = degrees.Where(e => e.DegreeId == Convert.ToInt32(request.Model.SelectedDegree)).Select(e => e.Name).FirstOrDefault();
                if (degreeName.Contains('/'))
                {
                    if (Convert.ToInt32(request.Model.SelectedSecondDegreeMajor) == 0)
                    {
                        return new RegistrationCommandResponse() { IsSuccessful = false, ErrorMessage = "Second degree major is required." , EncryptedUID = encryptedUID };
                    }
                }
                request.Model.Firstname = textinfo.ToTitleCase(request.Model.Firstname.ToLower());
                request.Model.Middlename = !string.IsNullOrEmpty(request.Model.Middlename) ? textinfo.ToTitleCase(request.Model.Middlename.ToLower()) : null;
                request.Model.Lastname = textinfo.ToTitleCase(request.Model.Lastname.ToLower());
                request.Model.SSN = _crypto.Encrypt(request.Model.SSN, GlobalConfigSettings);
                var studentID = await _repo.AddOrUpdateStudent(request.Model);

               
                string emailContent = "A Student Registration has been submitted.";
                if (await _featureManager.IsEnabledSiteWideAsync("UnregisteredStudentFlow"))
                {
                    string baseUrl = _config["General:BaseUrl"];
                    var secureLink = $"{baseUrl}/Admin/StudentProfileEdit?sid={studentID}";
                    emailContent = $@"Hello SFS Admin, <br /><br /> Student, {request.Model.Firstname} {request.Model.Lastname}, has submitted a student registration form. <br /><br />
                                         Please log in and verify the information on the student's profile via the following link:<br/><br/>
                                         {secureLink}<br/><br/>
                                         If the information provided is correct, please select ""Active"" in the account status dropdown. <br/><br/>
                                         If the information provided is not correct, please select ""Pending"" in the account status dropdown and contact the student at {request.Model.Email}. <br/><br/>";                                       
                }

                string toEmail = GlobalConfigSettings.Where(m => m.Key == "ProgramOfficeURI" && m.Type == "EmailSettings").Select(m => m.Value).FirstOrDefault();
                _ = await _emailer.SendEmailDefaultTemplateAsync(toEmail, $"SFS Student Registration: {request.Model.Firstname} {request.Model.Lastname}", emailContent);
                return new RegistrationCommandResponse() { IsSuccessful = true, ErrorMessage = "" };

                
            }
        }


    }
}
