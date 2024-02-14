using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.DTO;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Mappings;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.Pages.Admin
{

    [Authorize(Roles = "AD")]
    public class StudentProfileEditModel : PageModel
    {
        [BindProperty]
        public AdminStudentProfileEditViewModel Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;

        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        private readonly IMediator _mediator;
        public StudentProfileEditModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { Id = StudentID });
            if (!string.IsNullOrWhiteSpace(IsSuccess) && IsSuccess == "true") Data.ShowSuccessMessage = true;
        }

        public async Task<IActionResult> OnGetUnlinkAccount(int id)
        {
            await _mediator.Send(new UnlinkAccountRequest() { StudentID = id });
            return Redirect($"/Admin/StudentProfileEdit?sid={id}");
        }
        public async Task<IActionResult> OnGetDeleteAccount(int id)
        {
            await _mediator.Send(new DeleteAccountRequest() { StudentID = id });
            return Redirect($"/Admin/StudentLookup");
        }
        public async Task<IActionResult> OnPostSubmitAsync()
        {
            if (!ModelState.IsValid)
            {
				Data = await _mediator.Send(new Query() { Id = Data.StudentID });

				return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, AdminUserID = User.GetUserId() });
            return RedirectToPage("StudentProfileEdit", new { sid = Data.StudentID, s = "true" });
        }

        public class AdminStudentProfileEditValidator : AbstractValidator<AdminStudentProfileEditViewModel>
        {
            public AdminStudentProfileEditValidator()
            {
                RuleFor(x => x.Firstname).NotEmpty().Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.Lastname).NotEmpty().Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.DateOfBirth).NotEmpty().Matches("^(1[0-2]|0?[1-9])/(3[01]|[12][0-9]|0?[1-9])/[0-9]{4}$").WithName("Date of Birth").WithMessage("Date of birth is invalid date format."); 
                When(x => x.LoginGovID == null, () =>
                {
                    RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches("^[^><&]+$").WithName("Email");
                });
                RuleFor(x => x.AlternateEmail).NotEmpty().EmailAddress().Matches("^[^><&]+$").WithName("Alternate Email");
				
				RuleFor(x => x.CurrAddress1).NotEmpty().Matches("^[^><&]+$").WithName("Address");
                RuleFor(x => x.CurrCity).NotEmpty().Matches("^[^><&]+$").WithName("City");
                RuleFor(x => x.CurrStateID).NotEmpty().WithName("State");
                RuleFor(x => x.CurrPostalCode).NotEmpty().Matches("^[^><&]+$").WithName("Zip Code");
                RuleFor(x => x.CurrPhone).NotEmpty().Matches("^[^><&]+$").WithName("Phone");
                RuleFor(x => x.PermAddress1).NotEmpty().Matches("^[^><&]+$").WithName("Address");
                RuleFor(x => x.PermCity).NotEmpty().Matches("^[^><&]+$").WithName("City");
                RuleFor(x => x.PermStateID).NotEmpty().WithName("State");
                RuleFor(x => x.PermPostalCode).NotEmpty().Matches("^[^><&]+$").WithName("Zip Code");
                RuleFor(x => x.ContactFirstname).NotEmpty().Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.ContactLastname).NotEmpty().Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.ContactRelationship).NotEmpty().Matches("^[^><&]+$").WithName("Relationship");
                RuleFor(x => x.ContactPhone).NotEmpty().Matches("^[^><&]+$").WithName("Phone");
                RuleFor(x => x.Middlename).Matches("^[^><&]+$").WithName("Middle name");
                RuleFor(x => x.Suffix).Matches("^[^><&]+$").WithName("Suffix");
                RuleFor(x => x.CurrAddress2).Matches("^[^><&]+$").WithName("Address");
                RuleFor(x => x.CurrCountry).Matches("^[^><&]+$").WithName("Country");
                RuleFor(x => x.CurrExtension).Matches("^[^><&]+$").WithName("Extension");
                RuleFor(x => x.CurrFax).Matches("^[^><&]+$").WithName("Fax");
                RuleFor(x => x.CurrAddress2).Matches("^[^><&]+$").WithName("Address");
                RuleFor(x => x.PermCountry).Matches("^[^><&]+$").WithName("Country");
                RuleFor(x => x.PermExtension).Matches("^[^><&]+$").WithName("Extension");
                RuleFor(x => x.PermFax).Matches("^[^><&]+$").WithName("Fax");
                RuleFor(x => x.PermOtherPhone).Matches("^[^><&]+$").WithName("Phone");
                RuleFor(x => x.PermExtension).Matches("^[^><&]+$").WithName("Extension");
                RuleFor(x => x.ContactMiddlename).Matches("^[^><&]+$").WithName("Middle name");
                RuleFor(x => x.ContactEmail).EmailAddress().Matches("^[^><&]+$").WithName("Email");
                RuleFor(x => x.ContactExtension).Matches("^[^><&]+$").WithName("Extension");
                RuleForEach(x => x.Fundings).ChildRules(funding =>
                {
					funding.RuleFor(x => x.ExpectedGradDate).NotEmpty().Matches("^[^><&]+$").WithName("Expected Grad Date");
					funding.RuleFor(x => x.DateAvailIntern).NotEmpty().Matches("^[^><&]+$").WithName("Date Avail Internship");
					funding.RuleFor(x => x.DateAvailPostGrad).NotEmpty().Matches("^[^><&]+$").WithName("Date Avail Post Grad");
					funding.RuleFor(x => x.SelectedCollege).GreaterThan(0).WithMessage("Institition");
					funding.RuleFor(x => x.EnrolledSession).NotEmpty().Matches("^[^><&]+$");
					funding.RuleFor(x => x.FundingEndSession).NotEmpty().Matches("^[^><&]+$");
					funding.RuleFor(x => x.FundingEndYear).NotEmpty().GreaterThanOrEqualTo(x => x.EnrolledYear);
				});
            }
        }

        public class Query : IRequest<AdminStudentProfileEditViewModel>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminStudentProfileEditViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IStudentProfileMappingHelper _mapper;



            public QueryHandler(ScholarshipForServiceContext efDB, IStudentProfileMappingHelper mapper)
            {
                _efDB = efDB;
                _mapper = mapper;
            }

            public async Task<AdminStudentProfileEditViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                //Using automapper to build a select clause with ProjectTo
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<StudentInstitutionFunding, FundingDTO>();
                    cfg.CreateMap<SFS.Data.Student, StudentProfileDTO>();
                    cfg.CreateMap<SFS.Data.StudentSecurityCertification, SecurityClearanceDTO>();
                    cfg.CreateMap<SFS.Data.Address, AddressDTO>();
                    cfg.CreateMap<SFS.Data.Contact, ContactDTO>();
                });

                var data = await _efDB.Students.Where(m => m.StudentId == request.Id)
                    .ProjectTo<StudentProfileDTO>(config).AsSplitQuery().FirstOrDefaultAsync();

                //Splitting Funding query. For some reason Funding is not populating with the config on line 144...
                var fundingConfig = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<StudentInstitutionFunding, FundingDTO>();
                });

                var fundingData = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == request.Id)
                    .ProjectTo<FundingDTO>(fundingConfig).ToListAsync();

                data.StudentInstitionFundings = fundingData; //work around until we can use Automapper/ProjectTo with one configuration
                AdminStudentProfileEditViewModel vm = await _mapper.GetViewModelFromDTO(data, request.Id);
                return vm;
            }           

        }

        public class Command : IRequest<bool>
        {
            public int AdminUserID { get; set; }
            public AdminStudentProfileEditViewModel Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IAuditEventLogHelper _auditLogger;
            private readonly IStudentDashboardService _dashboardService;
            private readonly IReferenceDataRepository _refRepo;
			private readonly IFeatureManager _featureManager;
			private readonly IEmailerService _emailer;
			private readonly IConfiguration _config;

			public CommandHandler(ScholarshipForServiceContext efDB, IAuditEventLogHelper auditLogger, IStudentDashboardService dashService,
				IReferenceDataRepository refRepo, IFeatureManager featuerManager, IEmailerService emailer, IConfiguration config)
            {
                _efDB = efDB;
                _auditLogger = auditLogger;
                _dashboardService = dashService;
                _refRepo = refRepo;
                _featureManager= featuerManager;
                _emailer= emailer;
                _config= config;
            }
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                string currentStatus = "";
                var student = await _efDB.Students
                    .Where(m => m.StudentId == request.Model.StudentID)
                    .Include(m => m.StudentAccount)
                    .Include(m => m.CurrentAddress)
                    .Include(m => m.PermanentAddress)
                    .Include(m => m.EmergencyContact)
                    .Include((m => m.ProfileStatus))
                    .FirstOrDefaultAsync();
                var funding = await _efDB.StudentInstitutionFundings
                    .Include(m => m.Institution).ThenInclude(m => m.AcademicSchedule)
                    .Where(m => m.StudentId == request.Model.StudentID)
                    .Include(m => m.Institution)
                    .Include(m => m.Major)
                    .Include(m => m.Degree)
                    .FirstOrDefaultAsync();

                currentStatus = student.ProfileStatus.Name; 
                student.ProfileStatusID = request.Model.ProfileStatusID;
                student.FirstName = textinfo.ToTitleCase(request.Model.Firstname.ToLower());
                student.MiddleName = !string.IsNullOrWhiteSpace(request.Model.Middlename) ? textinfo.ToTitleCase(request.Model.Middlename.ToLower()) : "";
                student.LastName = textinfo.ToTitleCase(request.Model.Lastname.ToLower());
                student.Suffix = request.Model.Suffix;
                student.DateOfBirth = DateTime.Parse(request.Model.DateOfBirth);
                foreach (var f in request.Model.Fundings)
                {
					var fundingtoUpdate = await _efDB.StudentInstitutionFundings
	                .Include(m => m.Institution).ThenInclude(m => m.AcademicSchedule)
                	.Where(m => m.StudentInstitutionFundingId == f.ID)
	                .Include(m => m.Institution)
                	.Include(m => m.Major)
	                .Include(m => m.Degree)
	                .FirstOrDefaultAsync();
					fundingtoUpdate.InstitutionId = Convert.ToInt32(f.SelectedCollege);
					fundingtoUpdate.EnrolledSession = f.EnrolledSession;
					fundingtoUpdate.EnrolledYear = f.EnrolledYear;
					fundingtoUpdate.FundingEndSession = f.FundingEndSession;
					fundingtoUpdate.FundingEndYear = f.FundingEndYear;
                    fundingtoUpdate.MajorId = Convert.ToInt32(f.SelectedDiscipline);
                    fundingtoUpdate.DegreeId = Convert.ToInt32(f.SelectedDegree);
                    if (f.SelectedMinor.HasValue && f.SelectedMinor > 0)
                        fundingtoUpdate.MinorId = Convert.ToInt32(f.SelectedMinor.Value);
                    if (f.SelectedSecondDegreeMajor.HasValue && f.SelectedSecondDegreeMajor > 0)
                        fundingtoUpdate.SecondDegreeMajorId = Convert.ToInt32(f.SelectedSecondDegreeMajor.Value);
                    if (f.SelectedSecondDegreeMinor.HasValue && f.SelectedSecondDegreeMinor > 0)
                        fundingtoUpdate.SecondDegreeMinorId = Convert.ToInt32(f.SelectedSecondDegreeMinor.Value);

                    string academicSchedule = _efDB.Institutions.Where(m => m.InstitutionId == f.SelectedCollege).Select(m => m.AcademicSchedule.Name).FirstOrDefault();

                    string totalTerms = _dashboardService.CalculateTotalTerms(f.EnrolledYear.ToString(), f.EnrolledSession, f.FundingEndYear.ToString(), f.FundingEndSession, academicSchedule);
                    var strServiceOwed = _dashboardService.GetServiceOwed(academicSchedule, totalTerms, f.EnrolledYear.ToString(), f.EnrolledSession, f.FundingEndYear.ToString(), f.FundingEndSession);
                    double servieOwedValue;
                    var isDouble = double.TryParse(strServiceOwed, out servieOwedValue);
                    fundingtoUpdate.ExpectedGradDate = Convert.ToDateTime(f.ExpectedGradDate);
                    fundingtoUpdate.InternshipAvailDate = Convert.ToDateTime(f.DateAvailIntern);
                    fundingtoUpdate.PostGradAvailDate = Convert.ToDateTime(f.DateAvailPostGrad);
                    fundingtoUpdate.ServiceOwed = servieOwedValue > 0 ? servieOwedValue : null;
                    fundingtoUpdate.TotalAcademicTerms = totalTerms;
                }

				if (string.IsNullOrWhiteSpace(student.LoginGovLinkID))
                {
                    student.StudentAccount.UserName = request.Model.Email;
                    student.Email = request.Model.Email;
                }
                student.AlternateEmail = request.Model.AlternateEmail;

                if (student.CurrentAddressId.HasValue && student.CurrentAddressId.Value > 0)
                {
                    student.CurrentAddress.LineOne = request.Model.CurrAddress1;
                    student.CurrentAddress.LineTwo = request.Model.CurrAddress2;
                    student.CurrentAddress.City = request.Model.CurrCity;
                    student.CurrentAddress.StateId = request.Model.CurrStateID;
                    student.CurrentAddress.PostalCode = request.Model.CurrPostalCode;
                    student.CurrentAddress.Country = request.Model.CurrCountry;
                    student.CurrentAddress.PhoneNumber = request.Model.CurrPhone;
                    student.CurrentAddress.PhoneExtension = request.Model.CurrExtension;
                    student.CurrentAddress.Fax = request.Model.CurrFax;
                }
                else
                {
                    student.CurrentAddress = new Address()
                    {
                        LineOne = request.Model.CurrAddress1,
                        LineTwo = request.Model.CurrAddress2,
                        City = request.Model.CurrCity,
                        StateId = request.Model.CurrStateID,
                        PostalCode = request.Model.CurrPostalCode,
                        Country = request.Model.CurrCountry,
                        PhoneNumber = request.Model.CurrPhone,
                        PhoneExtension = request.Model.CurrExtension,
                        Fax = request.Model.CurrFax
                    };
                }

                if (request.Model.PermUseCurretAddressAsPerm)
                {
                    if (student.PermanentAddressId.HasValue && student.PermanentAddressId.Value > 0)
                    {

                        student.PermanentAddress.LineOne = request.Model.CurrAddress1;
                        student.PermanentAddress.LineTwo = request.Model.CurrAddress2;
                        student.PermanentAddress.City = request.Model.CurrCity;
                        student.PermanentAddress.StateId = request.Model.CurrStateID;
                        student.PermanentAddress.PostalCode = request.Model.CurrPostalCode;
                        student.PermanentAddress.Country = request.Model.CurrCountry;
                        student.PermanentAddress.PhoneNumber = request.Model.CurrPhone;
                        student.PermanentAddress.PhoneExtension = request.Model.CurrExtension;
                        student.PermanentAddress.Fax = request.Model.CurrFax;
                    }
                    else
                    {
                        student.PermanentAddress = new Address()
                        {
                            LineOne = request.Model.CurrAddress1,
                            LineTwo = request.Model.CurrAddress2,
                            City = request.Model.CurrCity,
                            StateId = request.Model.CurrStateID,
                            PostalCode = request.Model.CurrPostalCode,
                            Country = request.Model.CurrCountry,
                            PhoneNumber = request.Model.CurrPhone,
                            PhoneExtension = request.Model.CurrExtension,
                            Fax = request.Model.CurrFax
                        };
                    }
                }
                else
                {
                    if (student.PermanentAddressId.HasValue && student.PermanentAddressId.Value > 0)
                    {

                        student.PermanentAddress.LineOne = request.Model.PermAddress1;
                        student.PermanentAddress.LineTwo = request.Model.PermAddress2;
                        student.PermanentAddress.City = request.Model.PermCity;
                        student.PermanentAddress.StateId = request.Model.PermStateID;
                        student.PermanentAddress.PostalCode = request.Model.PermPostalCode;
                        student.PermanentAddress.Country = request.Model.PermCountry;
                        student.PermanentAddress.PhoneNumber = request.Model.PermPhone;
                        student.PermanentAddress.PhoneExtension = request.Model.PermExtension;
                        student.PermanentAddress.Fax = request.Model.PermFax;
                    }
                    else
                    {
                        student.PermanentAddress = new Address()
                        {
                            LineOne = request.Model.PermAddress1,
                            LineTwo = request.Model.PermAddress2,
                            City = request.Model.PermCity,
                            StateId = request.Model.PermStateID,
                            PostalCode = request.Model.PermPostalCode,
                            Country = request.Model.PermCountry,
                            PhoneNumber = request.Model.PermPhone,
                            PhoneExtension = request.Model.PermExtension,
                            Fax = request.Model.PermFax
                        };
                    }
                }

                if (student.EmergencyContactId.HasValue && student.EmergencyContactId.Value > 0)
                {
                    student.EmergencyContact.FirstName = request.Model.ContactFirstname;
                    student.EmergencyContact.MiddleName = request.Model.ContactMiddlename;
                    student.EmergencyContact.LastName = request.Model.ContactLastname;
                    student.EmergencyContact.Relationship = request.Model.ContactRelationship;
                    student.EmergencyContact.Email = request.Model.ContactEmail;
                    student.EmergencyContact.Phone = request.Model.ContactPhone;
                    student.EmergencyContact.PhoneExt = request.Model.ContactExtension;

                }
                else
                {
                    student.EmergencyContact = new Contact()
                    {
                        FirstName = request.Model.ContactFirstname,
                        MiddleName = request.Model.ContactMiddlename,
                        LastName = request.Model.ContactLastname,
                        Relationship = request.Model.ContactRelationship,
                        Email = request.Model.ContactEmail,
                        Phone = request.Model.ContactPhone,
                        PhoneExt = request.Model.ContactExtension
                    };
                }


                //Certificates remove all and re-add
                var currentCerts = _efDB.StudentSecurityCertifications.Where(m => m.StudentId == request.Model.StudentID).ToList();
                if (currentCerts != null && currentCerts.Any())
                {
                    _efDB.StudentSecurityCertifications.RemoveRange(currentCerts);
                }

                if (request.Model.SelectedCertificates != null && request.Model.SelectedCertificates.Any())
                {
                    foreach (var item in request.Model.SelectedCertificates)
                    {
                        student.StudentSecurityCertifications.Add(new StudentSecurityCertification() { SecurityCertificationId = item });
                    }
                }

          
                student.LastUpdated = DateTime.UtcNow;
                student.UpdatedByID = request.AdminUserID.ToString();

                await _efDB.SaveChangesAsync();
                await _auditLogger.LogAuditEvent("Admin Update Student Profile");
                return true;

            }


        }

        public class UnlinkAccountRequest : IRequest<bool>
        {
            public int StudentID { get; set; }
        }

        public class UnlinkAccountRequestHandler : IRequestHandler<UnlinkAccountRequest, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public UnlinkAccountRequestHandler(ScholarshipForServiceContext efDB)
            {
                _efDB = efDB;
            }
            public async Task<bool> Handle(UnlinkAccountRequest request, CancellationToken cancellationToken)
            {
                var studentToUpdate = await _efDB.Students.Where(m => m.StudentId == request.StudentID).FirstOrDefaultAsync();
                studentToUpdate.LoginGovLinkID = null;
                await _efDB.SaveChangesAsync();
                return true;
            }
        }

        public class DeleteAccountResult
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
        }
        public class DeleteAccountRequest : IRequest<DeleteAccountResult>
        {
            public int StudentID { get; set; }
        }
        public class DeleteAccountRequestHandler : IRequestHandler<DeleteAccountRequest, DeleteAccountResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public readonly IAuditEventLogHelper _auditLogger;
            public DeleteAccountRequestHandler(ScholarshipForServiceContext efDB, IAuditEventLogHelper audit)
            {
                _efDB = efDB;
                _auditLogger = audit;
            }
            public async Task<DeleteAccountResult> Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
            {
                var studentToDelete = await _efDB.Students.Where(m => m.StudentId == request.StudentID).FirstOrDefaultAsync();
                var studentAccount = await _efDB.StudentAccount.Where(m => m.StudentID == request.StudentID).FirstOrDefaultAsync();
                var StudentResumes = _efDB.StudentBuilderResumes.Where(m => m.StudentId == request.StudentID);
                var StudentCommitments = _efDB.StudentCommitments.Where(m => m.StudentId == request.StudentID);
                var StudentDocuments = _efDB.StudentDocuments.Where(m => m.StudentId == request.StudentID);
                var StudentInstituteFunding = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == request.StudentID).FirstOrDefaultAsync();
                var StudentJobActivity = await _efDB.StudentJobActivities.Where(m => m.StudentId == request.StudentID).FirstOrDefaultAsync();
                var StudentRace = await _efDB.StudentRaces.Where(m => m.StudentId == request.StudentID).FirstOrDefaultAsync();
                var StudentSecurityCerifcations = await _efDB.StudentSecurityCertifications.Where(m => m.StudentId == request.StudentID).FirstOrDefaultAsync();
                var StudentCurrentAddress = _efDB.Addresses.Where(m => m.AddressId == studentToDelete.CurrentAddressId);
                var StudentPermAddress = _efDB.Addresses.Where(m => m.AddressId == studentToDelete.PermanentAddressId);
                var StudentEmergencyContact = _efDB.Contacts.Where(m => m.ContactId == studentToDelete.EmergencyContactId);


                if (studentToDelete.StudentId > 0 && studentToDelete.StudentUID > 0)
                {
                    if (StudentSecurityCerifcations != null)
                        _efDB.StudentSecurityCertifications.Remove(StudentSecurityCerifcations);
                    if (StudentRace != null)
                        _efDB.StudentRaces.Remove(StudentRace);
                    if (StudentJobActivity != null)
                        _efDB.StudentJobActivities.Remove(StudentJobActivity);
                    if (StudentResumes != null)
                        _efDB.StudentBuilderResumes.RemoveRange(StudentResumes);
                    if (StudentDocuments != null)
                        _efDB.StudentDocuments.RemoveRange(StudentDocuments);
                    if (StudentCommitments != null)
                        _efDB.StudentCommitments.RemoveRange(StudentCommitments);
                    if (StudentCurrentAddress != null)
                        _efDB.Addresses.RemoveRange(StudentCurrentAddress);
                    if (StudentPermAddress != null)
                        _efDB.Addresses.RemoveRange(StudentPermAddress);
                    if (StudentEmergencyContact != null)
                        _efDB.Contacts.RemoveRange(StudentEmergencyContact);
                    _efDB.StudentInstitutionFundings.Remove(StudentInstituteFunding);
                    _efDB.StudentAccount.Remove(studentAccount);
                    _efDB.Students.Remove(studentToDelete);
                    await _efDB.SaveChangesAsync();
                    await _auditLogger.LogAuditEvent($"SFS Admin: Student Account {studentToDelete.StudentId} was successfully deleted from SFS System");
                    return new DeleteAccountResult() { IsSuccess = true, ErrorMessage = "Student Account deletion successfully." };

                }
                await _auditLogger.LogAuditEvent($"SFS Admin: deletion attempt for student account {studentToDelete.StudentId} was unsuccessful");
                return new DeleteAccountResult() { IsSuccess = true, ErrorMessage = "Student Account deletion failed." };
            }
        }
    }

}
