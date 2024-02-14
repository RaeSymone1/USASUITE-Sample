using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Core.Shared;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Web.Shared;
using System.Globalization;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Models.Academia;

namespace OPM.SFS.Web.Pages.Student
{


    [Authorize(Roles = "ST")]
    public class ProfileModel : PageModel
    {

        private readonly IMediator _mediator;
        public ProfileModel(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public StudentProfileViewModel Data { get; set; }

        [FromQuery(Name = "i")]
        public string ShowIncompleteWarning { get; set; }

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { Id = User.GetUserId() });
            if (!string.IsNullOrWhiteSpace(ShowIncompleteWarning) && ShowIncompleteWarning == "true") Data.ShowIncompleteWarning = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {            
            if (!ModelState.IsValid)
            {
                Data.Certificates = new();
				Data = await _mediator.Send(new Query() { Id = User.GetUserId() });
				return Page();
            }
            var response = await _mediator.Send(new Command() {  Model = Data, Id = User.GetUserId() });
            return RedirectToPage("Dashboard");
        }

        public class ProfileValidator : AbstractValidator<StudentProfileViewModel>
        {
            public ProfileValidator()
            {

                RuleFor(x => x.Firstname).NotEmpty().Matches("^[^><&]+$").MaximumLength(255).WithName("First name");
                RuleFor(x => x.Lastname).NotEmpty().Matches("^[^><&]+$").MaximumLength(50).WithName("Last name");
                RuleFor(x => x.DateOfBirth).NotEmpty().Matches("^[^><&]+$").WithName("Date of Birth");
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
                RuleFor(x => x.PermPhone).NotEmpty().Matches("^[^><&]+$").WithName("Phone");
                RuleFor(x => x.ContactFirstname).NotEmpty().Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.ContactLastname).NotEmpty().Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.ContactRelationship).NotEmpty().Matches("^[^><&]+$").WithName("Relationship");
                RuleFor(x => x.ContactPhone).NotEmpty().Matches("^[^><&]+$").WithName("Phone");
                RuleFor(x => x.ContactEmail).NotEmpty().EmailAddress().Matches("^[^><&]+$").WithName("Email");
                RuleFor(x => x.Middlename).Matches("^[^><&]+$").MaximumLength(50).WithName("Middle name");
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
                RuleFor(x => x.ContactExtension).Matches("^[^><&]+$").WithName("Extension");
				RuleForEach(x => x.InstituteFundings).ChildRules(funding =>
				{
					funding.RuleFor(x => x.DateAvailIntern).NotEmpty().Matches("^[^><&]+$").WithName("Date Avail Internship");
					funding.RuleFor(x => x.DateAvailPostGrad).NotEmpty().Matches("^[^><&]+$").WithName("Date Avail Post Grad");
				});


			}
        }

        public class Query : IRequest<StudentProfileViewModel>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, StudentProfileViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IReferenceDataRepository _refRepo;
            private readonly ICryptoHelper _crypto;
            private readonly IConfiguration _appSettings;


            public QueryHandler(ScholarshipForServiceContext efDB, ICryptoHelper crypto, IConfiguration appSettings, IReferenceDataRepository refRepo)
            {
                _efDB = efDB;
                _crypto = crypto;
                _appSettings = appSettings;
                _refRepo = refRepo;
            }

            public async Task<StudentProfileViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _efDB.Students.AsNoTracking()
                    .Where(m => m.StudentId == request.Id)
                    .Include(m => m.CurrentAddress)
                    .Include(m => m.PermanentAddress)
                    .Include(m => m.EmergencyContact)
                    .Include(m => m.StudentSecurityCertifications)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
				    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Degree)
				    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Major)
				    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Minor)
					.FirstOrDefaultAsync();

                var vm = new StudentProfileViewModel();
                vm.StateList = new SelectList(await _refRepo.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
				var degreeList = await _refRepo.GetDegreesAsync();

				vm.LoginGovEditUrl = _appSettings["LoginGov:EditUrl"];
                if (data != null)
                {
                    var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
                    vm.Firstname = data.FirstName;
                    vm.Middlename = data.MiddleName;
                    vm.Lastname = data.LastName;
                    vm.Suffix = data.Suffix;
                    vm.DateOfBirth = data.DateOfBirth.ToShortDateString();
                    var ssn = _crypto.Decrypt(data.Ssn, GlobalConfigSettings);
                    vm.Last4SSN = ssn.Substring(ssn.Length - 4);
                    vm.Email = data.Email;
                    vm.AlternateEmail = data.AlternateEmail;
                    
                    vm.DateAvailIntern = data.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate.HasValue ? $"{data.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate.Value.Month}/{data.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate.Value.Year}" : ""; ;
                    vm.DateAvailPostGrad = data.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate.HasValue ? $"{data.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate.Value.Month}/{data.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate.Value.Year}" : ""; ;
                    vm.Certificates = new();
                    var certs = await _refRepo.GetSecurityCertificationAsync();
                    vm.InstituteFundings = new();
                    foreach(var c in certs)
                    {
                        if(data.StudentSecurityCertifications.Where(m => m.SecurityCertificationId == c.SecurityCertificationId).Any())
                        {
                            vm.Certificates.Add(new Certificate()
                            {
                                ID = c.SecurityCertificationId,
                                Name = c.SecurityCertificationName,
                                Selected = true
                            });
                        }
                        else
                        {
                            vm.Certificates.Add(new Certificate()
                            {
                                ID = c.SecurityCertificationId,
                                Name = c.SecurityCertificationName,
                                Selected = false
                            });
                        }                      
                    }

					if (data.StudentInstitutionFundings != null)
					{
						foreach (var f in data.StudentInstitutionFundings)
						{
							var degreeName = degreeList.Where(m => m.DegreeId == f.DegreeId).Select(m => m.Name).FirstOrDefault();


							vm.InstituteFundings.Add(new StudentProfileViewModel.InstituteFunding()							
                            {
								ID = f.StudentInstitutionFundingId,
								University = f.Institution.Name,
								Discipline = f.Major != null ? $"{f.Major.Name}" : "",
								Degree = f.Degree != null ? $"{f.Degree.Name}" : "",

								ExpectedGradDate = f.ExpectedGradDate.HasValue ? $"{f.ExpectedGradDate.Value.Month}/{f.ExpectedGradDate.Value.Year}" : "",
								DateAvailIntern = f.InternshipAvailDate.HasValue ? $"{f.InternshipAvailDate.Value.Month}/{f.InternshipAvailDate.Value.Year}" : "",
								DateAvailPostGrad = f.PostGradAvailDate.HasValue ? $"{f.PostGradAvailDate.Value.Month}/{f.PostGradAvailDate.Value.Year}" : "",
								Minor = f.Minor != null ? $"{f.Minor.Name}" : "",
								SecondDegreeMajor = f.SecondDegreeMajor != null ? $"{f.SecondDegreeMajor.Name}" : "",
								SecondDegreeMinor = f.SecondDegreeMinor != null ? $"{f.SecondDegreeMinor.Name}" : "",
								ShowSecondDegreeInfo = degreeName.Contains("/") ? true : false
							});

						}
					}

					if (data.CurrentAddress != null)
                    {
                        vm.CurrAddress1 = data.CurrentAddress.LineOne;
                        vm.CurrAddress2 = data.CurrentAddress.LineTwo;
                        vm.CurrCity = data.CurrentAddress.City;
                        vm.CurrStateID = data.CurrentAddress.StateId;
                        vm.CurrPostalCode = data.CurrentAddress.PostalCode;
                        vm.CurrCountry = data.CurrentAddress.Country;
                        vm.CurrPhone = data.CurrentAddress.PhoneNumber;
                        vm.CurrExtension = data.CurrentAddress.PhoneExtension;
                        vm.CurrFax = data.CurrentAddress.Fax;
                    }

                    if (data.PermanentAddress != null)
                    {
                        vm.PermAddress1 = data.PermanentAddress.LineOne;
                        vm.PermAddress2 = data.PermanentAddress.LineTwo;
                        vm.PermCity = data.PermanentAddress.City;
                        vm.PermPostalCode = data.PermanentAddress.PostalCode;
                        vm.PermCountry = data.PermanentAddress.Country;
                        vm.PermStateID = data.PermanentAddress.StateId;
                        vm.PermPhone = data.PermanentAddress.PhoneNumber;
                        vm.PermExtension = data.PermanentAddress.PhoneExtension;
                    }
                    //What is Other Phone on the profile?? TBD
                    if (data.EmergencyContactId.HasValue && data.EmergencyContactId.Value > 0)
                    {
                       
                        vm.ContactFirstname = data.EmergencyContact.FirstName;
                        vm.ContactMiddlename = data.EmergencyContact.MiddleName;
                        vm.ContactLastname = data.EmergencyContact.LastName;
                        vm.ContactRelationship = data.EmergencyContact.Relationship;
                        vm.ContactEmail = data.EmergencyContact.Email;
                        vm.ContactPhone = data.EmergencyContact.Phone;
                        vm.ContactExtension = data.EmergencyContact.PhoneExt;
                    }


                }
                return vm;
            }
        }

        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
            public StudentProfileViewModel Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IEmailerService _emailer;
            private readonly ICacheHelper _cache;
            private readonly IAuditEventLogHelper _auditLogger;

            public CommandHandler(ScholarshipForServiceContext efDB, IEmailerService emailer, ICacheHelper cache, IAuditEventLogHelper auditLogger)
            {
                _efDB = efDB;
                _emailer = emailer;
                _cache = cache;
                _auditLogger = auditLogger;
            }
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                var student = await _efDB.Students
                    .Where(m => m.StudentId == request.Id)
                    .Include(m => m.StudentAccount)
                    .Include(m => m.CurrentAddress)
                    .Include(m => m.PermanentAddress)
                    .Include(m => m.EmergencyContact)                   
                    .FirstOrDefaultAsync();



                student.FirstName = textinfo.ToTitleCase(request.Model.Firstname.ToLower());
                student.MiddleName = !string.IsNullOrWhiteSpace(request.Model.Middlename) ? textinfo.ToTitleCase(request.Model.Middlename.ToLower()) : "";
                student.LastName = textinfo.ToTitleCase(request.Model.Lastname.ToLower());
                student.Suffix = request.Model.Suffix;
                student.StudentAccount.UserName = request.Model.Email;
                student.AlternateEmail = request.Model.AlternateEmail;
                if(student.CurrentAddressId.HasValue && student.CurrentAddressId.Value > 0)
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

                if(student.EmergencyContactId.HasValue && student.EmergencyContactId.Value > 0)
                {
                    student.EmergencyContact.FirstName = !string.IsNullOrWhiteSpace(request.Model.ContactFirstname) ? textinfo.ToTitleCase(request.Model.ContactFirstname.ToLower()) : "";
                    student.EmergencyContact.MiddleName = !string.IsNullOrWhiteSpace(request.Model.ContactMiddlename) ? textinfo.ToTitleCase(request.Model.ContactMiddlename.ToLower()) : "";
                    student.EmergencyContact.LastName = !string.IsNullOrWhiteSpace(request.Model.ContactLastname) ? textinfo.ToTitleCase(request.Model.ContactLastname.ToLower()) : "";
                    student.EmergencyContact.Relationship = request.Model.ContactRelationship;
                    student.EmergencyContact.Email = request.Model.ContactEmail;
                    student.EmergencyContact.Phone = request.Model.ContactPhone;
                    student.EmergencyContact.PhoneExt = request.Model.ContactExtension;

                }
                else
                {
                    student.EmergencyContact = new Contact()
                    {
                        FirstName = !string.IsNullOrWhiteSpace(request.Model.ContactFirstname) ? textinfo.ToTitleCase(request.Model.ContactFirstname.ToLower()) : "",
                        MiddleName = !string.IsNullOrWhiteSpace(request.Model.ContactMiddlename) ? textinfo.ToTitleCase(request.Model.ContactMiddlename.ToLower()) : "",
                        LastName = !string.IsNullOrWhiteSpace(request.Model.ContactLastname) ? textinfo.ToTitleCase(request.Model.ContactLastname.ToLower()) : "",
                        Relationship = request.Model.ContactRelationship ?? "",
                        Email = request.Model.ContactEmail ?? "",
                        Phone = request.Model.ContactPhone ?? "",
                        PhoneExt = request.Model.ContactExtension ?? ""
                    };
                }


                //Certificates remove all and re-add
                var currentCerts = _efDB.StudentSecurityCertifications.Where(m => m.StudentId == request.Id).ToList();
                if (currentCerts != null && currentCerts.Any())
                {
                    _efDB.StudentSecurityCertifications.RemoveRange(currentCerts);
                }

                if (request.Model.SelectedCertificates != null && request.Model.SelectedCertificates.Any())
                {
                    foreach(var item in request.Model.SelectedCertificates)
                    {
                        student.StudentSecurityCertifications.Add(new StudentSecurityCertification() { SecurityCertificationId = item });
                    }
                }
				foreach (var f in request.Model.InstituteFundings)
				{
					var fundingtoUpdate = await _efDB.StudentInstitutionFundings
                        .Include(m => m.Institution).ThenInclude(m => m.AcademicSchedule)   
                        .Where(m => m.StudentInstitutionFundingId == f.ID)
                        .Include(m => m.Institution)
                        .Include(m => m.Major)
                        .Include(m => m.Degree)
                        .FirstOrDefaultAsync();
					fundingtoUpdate.InternshipAvailDate = Convert.ToDateTime(f.DateAvailIntern);
					fundingtoUpdate.PostGradAvailDate = Convert.ToDateTime(f.DateAvailPostGrad);
				}
				student.LastUpdated = DateTime.UtcNow;
                student.UpdatedByID = request.Id.ToString();

                await _auditLogger.LogAuditEvent("Student Profile Updated");
                await _efDB.SaveChangesAsync();
                string selectedCerts = "N/A";
                var certs = await _cache.GetSecurityCertificationAsync();
                if (student.StudentSecurityCertifications != null && student.StudentSecurityCertifications.Count > 0)
                {
                    selectedCerts = "";
                    foreach (var cert in student.StudentSecurityCertifications)
                        selectedCerts += $"{certs.Where(m => m.SecurityCertificationId == cert.SecurityCertificationId).Select(m => m.SecurityCertificationName).FirstOrDefault()},";
                }
                //string emailContent = $@"<b>{student.FirstName} {student.MiddleName} {student.LastName}'s </b> student profile has been updated successfully.  Below is some of the student’s profile information., <br /><br />
                //                            <b>Unique ID: </b><i> {student.StudentUID} </i><br /><br />
                //                            <b>Email Address: </b><i>{student.Email}</i><br /><br />
                //                            <b>Alternate Email Address: </b><i>{(student.AlternateEmail == "" ? "N/A" : student.AlternateEmail)}</i><br /><br />
                //                            <b>Sponsor/Partner School: </b><i>{funding.Institution.Name }</i><br /><br />
                //                            <b>Degree Program: </b><i>{funding.Degree.Name}</i><br /><br />
                //                            <b>Security Certifications: </b><i>{selectedCerts}</i><br /><br />
                //                            <b>Discipline: </b><i>{funding.Major.Name}</i><br /><br />
                //                            <b>Expected Graduation Date (mm/yyyy): </b><i>{funding.ExpectedGradDate.Value.Month}/{funding.ExpectedGradDate.Value.Year}</i><br /><br />
                //                            <b>Date Available for Internship (mm/yyyy): </b><i>{funding.InternshipAvailDate.Value.Month}/{funding.InternshipAvailDate.Value.Year}</i><br /><br />
                //                            <b>Date Available for Post-Grad Commitment (mm/yyyy): </b><i>{funding.PostGradAvailDate.Value.Month}/{funding.PostGradAvailDate.Value.Year}</i><br /><br />";

                //await _emailer.SendEmailNoTemplateAsync(student.Email, "SFS Student Profile Update Notification", emailContent);
                return true;
               
            }
        }

       

    }
    
}
