using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Models.Agency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.Pages.Agency
{
    [Authorize(Roles = "AO")]
    public class ResumeViewModel : PageModel
    {
        [BindProperty]
        public AgencyOfficialStudentResumeVM Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;

        private readonly IMediator _mediator;

        public ResumeViewModel(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentId = StudentID });
            if(Data.TheUploadedResume is not null)
            {
                var file = await _mediator.Send(new QueryDocumentView() { UploadedResume = Data.TheUploadedResume });
                return file;
            }
            return Page();
        }

     
        public class Query : IRequest<AgencyOfficialStudentResumeVM>
        {
            public int StudentId { get; set; }
        }
              

        public class QueryHandler : IRequestHandler<Query, AgencyOfficialStudentResumeVM>
        {
            private readonly ScholarshipForServiceContext _db;
            public QueryHandler(ScholarshipForServiceContext db) => _db = db;

            public async Task<AgencyOfficialStudentResumeVM> Handle(Query request, CancellationToken cancellationToken)
            {
               
                var model = new AgencyOfficialStudentResumeVM();
                var resumeLookup = await _db.StudentDocuments.Where(m => m.IsShareable == true && m.StudentId == request.StudentId)
                    .Select(m => new
                    {
                        FilePath = m.FilePath,
                        FileName = m.FileName
                    })
                    .FirstOrDefaultAsync();

                if (resumeLookup is null || string.IsNullOrWhiteSpace(resumeLookup.FilePath))
                {
                    var data = await _db.Students.Where(m => m.StudentId == request.StudentId)
                   .Select(stud => new
                   {
                       Fullname = $"{stud.FirstName} {stud.LastName}",
                       FullAddress = $"{stud.PermanentAddress.LineOne}, {stud.PermanentAddress.State.Name}, {stud.PermanentAddress.PostalCode}",
                       Phone = stud.PermanentAddress.PhoneNumber,
                       OtherPhone = stud.CurrentAddress.PhoneNumber,
                       Fax = stud.PermanentAddress.Fax,
                       Email = stud.Email,
                       AltEmail = stud.AlternateEmail,
                       Objective = stud.StudentBuilderResumes.FirstOrDefault().Objective,
                       Coursework = stud.StudentBuilderResumes.FirstOrDefault().OtherQualification,
                       JobRelatedSkills = stud.StudentBuilderResumes.FirstOrDefault().JobRelatedSkill,
                       Certs = stud.StudentBuilderResumes.FirstOrDefault().Certificate,
                       Honors = stud.StudentBuilderResumes.FirstOrDefault().HonorsAwards,
                       Supplemental = stud.StudentBuilderResumes.FirstOrDefault().Supplemental,
                       WorkExperience = stud.StudentBuilderResumes.FirstOrDefault().WorkExperiences.Select(m => new
                       {
                           Employer = m.Employer,
                           Title = m.Title,
                           Dates = $"{m.Start} to {m.End}",
                           AddressLine = $"{m.Address.City}, {m.Address.State.Abbreviation}",
                           Hours = m.HoursPerWeek,
                           Salary = m.Salary,
                           Duties = m.Duties
                       }),
                       Education = stud.StudentBuilderResumes.FirstOrDefault().Educations.Select(m => new
                       {
                           School = m.SchoolName,
                           AddressLine = $"{m.CityName}, {m.State.Abbreviation}",
                           Degree = m.Degree,
                           Major = m.Major,
                           Minor = m.Minor,
                           Honors = m.Honors
                       })
                   })
                   .FirstOrDefaultAsync();


                    model.TheBuilderResume = new();
                    
                    model.TheBuilderResume.FullName = data.Fullname;
                    model.TheBuilderResume.FullAddress = data.FullAddress;
                    model.TheBuilderResume.Email = data.Email;
                    model.TheBuilderResume.AltEmail = data.AltEmail;
                    model.TheBuilderResume.Phone = data.Phone;
                    model.TheBuilderResume.Objective = data.Objective;

                    if (data.WorkExperience is not null && data.WorkExperience.Count() > 0)
                    {
                        model.TheBuilderResume.WorkExperience = new List<AgencyOfficialStudentResumeVM.ViewWorkExperience>();
                        foreach (var w in data.WorkExperience)
                        {                           
                            model.TheBuilderResume.WorkExperience.Add(new AgencyOfficialStudentResumeVM.ViewWorkExperience()
                            {
                                Employer = w.Employer,
                                Title = w.Title,
                                Dates = w.Dates,
                                AddressLine = w.AddressLine,
                                Hours = w.Hours,
                                Salary = w.Salary,
                                Duties = w.Duties
                            });
                        }
                    }

                    if (data.Education is not null && data.Education.Count() > 0)
                    {
                        model.TheBuilderResume.Education = new List<AgencyOfficialStudentResumeVM.ViewEduction>();
                        foreach (var e in data.Education)
                        {
                            model.TheBuilderResume.Education.Add(new AgencyOfficialStudentResumeVM.ViewEduction()
                            {
                                School = e.School,
                                AddressLine = e.AddressLine,
                                Degree = e.Degree,
                                Major = e.Major,
                                Minor = e.Minor,
                                Honors = e.Honors
                            });
                        }
                    }
                    model.TheBuilderResume.Courses = data.Coursework;
                    model.TheBuilderResume.JobRelatedSkills = data.JobRelatedSkills;
                    model.TheBuilderResume.Certifications = data.Certs;
                    model.TheBuilderResume.Honors = data.Honors;
                    model.TheBuilderResume.Supplemental = data.Supplemental;
                    return model;
                }

                //Uploaded resume, set up download!
                model.TheUploadedResume = new();
                model.TheUploadedResume.FileName = resumeLookup.FileName;
                model.TheUploadedResume.FilePath = resumeLookup.FilePath;
                return model;
            }
        }

        public class QueryDocumentView : IRequest<FileStreamResult>
        {
            public AgencyOfficialStudentResumeVM.ViewUploadedResume UploadedResume { get; set; }
        }

        public class QueryDocumentViewHandler : IRequestHandler<QueryDocumentView, FileStreamResult>
        {
            private readonly IDocumentRepository _documentRepo;
            private readonly IAzureBlobService _blobService;
            private readonly IConfiguration _appSettings;

            public QueryDocumentViewHandler(IDocumentRepository document, IAzureBlobService blobService, IConfiguration appSettings)
            {
                _documentRepo = document;
                _blobService = blobService;
                _appSettings = appSettings;
            }

            public async Task<FileStreamResult> Handle(QueryDocumentView request, CancellationToken cancellationToken)
            {             
                if (_appSettings["General:Hosting"] == "Macon")
                {
                    var extension = System.IO.Path.GetExtension(request.UploadedResume.FilePath);
                    FileStream fileStream = new FileStream(request.UploadedResume.FilePath, FileMode.Open, FileAccess.Read);
                    var theFile = new FileStreamResult(fileStream, GetMimeType(extension));
                    theFile.FileDownloadName = $"{request.UploadedResume.FileName}{System.IO.Path.GetExtension(request.UploadedResume.FilePath)}";
                    return theFile;
                }
                else
                {
                    return await _blobService.DownloadDocumentStreamAsync(request.UploadedResume.FilePath, request.UploadedResume.FileName);
                }
            }

            private string GetMimeType(string filetype)
            {
                switch (filetype.ToLower().Replace(".", ""))
                {
                    case "pdf":
                        return "application/pdf";
                    case "doc":
                        return "application/msword";
                    case "docx":
                        return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    case "png":
                        return "image/png";
                    case "gif":
                        return "image/gif";
                    case "jpg":
                        return "image/jpeg";
                    case "pptx":
                        return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    case "ppt":
                        return "application/vnd.ms-powerpoint";
                    default:
                        break;
                }
                return "";
            }


        }
    }
}
