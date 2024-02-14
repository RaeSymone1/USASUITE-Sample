using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    [IsProfileCompleteFilter()]
    public class ResumeListModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public StudentResumeViewModel Data { get; set; }

        [FromQuery(Name = "fid")]
        public int DocumentId { get; set; } = 0;

        public ResumeListModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentID = User.GetUserId() });
        }

        public async Task<FileResult> OnGetViewDocument()
        {            
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = DocumentId, StudentId = User.GetUserId() });
            return file;
        }

        public async Task<IActionResult> OnPostUploadAsync(UploadResumeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                return new ObjectResult(new { status = error.ErrorMessage });

            }
            var result = await _mediator.Send(new CommandUpload() { UploadedResume = model.Resume, 
                StudentID = User.GetUserId(), 
                Name = model.Name });
            if(result.IsSuccess) return new ObjectResult(new { status = "success" });
            return new ObjectResult(new { status = result.ErrorMessage });

        }
      
        public async Task<ActionResult> OnPostDeleteAsync(string docid)
        {
            var result = await _mediator.Send(new CommandDelete() { DocumentID = Convert.ToInt32(docid), 
                StudentID = User.GetUserId() });
            return Redirect($"/Student/ResumeList");
           
        }

        public JsonResult OnGetShareResume(int docID)
        {
            var data = _mediator.Send(new CommandShareResume() { StudentID = User.GetUserId(), DocumentID = docID }).Result;
            return new JsonResult(data);
        }

        public class ResumeUploadValidator : AbstractValidator<UploadResumeViewModel>
        {
            public ResumeUploadValidator()
            {
                RuleFor(x => x.Name).Matches("^[^><&]+$");
                RuleFor(x => x.Resume).NotNull().WithMessage("Resume is required.");
                When(x => x.Resume != null, () =>
                {
                    RuleFor(x => x.Resume.Length).NotNull().LessThanOrEqualTo(5242880)
                        .WithMessage("File size exceeds 5MB");
                    RuleFor(x => x.Resume.ContentType).Must((o, positiondescription) => { return IsValidDocType(o.Resume.ContentType); })
                        .WithMessage("Files must be one of the following formats: TXT, PDF, Word (DOC or DOCX) or Excel (XLS, XSLX)"); ;
                });
            }

            private bool IsValidDocType(string conentType)
            {
                if (conentType.ToLower() == "application/pdf") //pdf
                    return true;
                if (conentType.ToLower() == "application/msword") //word (doc)
                    return true;
                if (conentType.ToLower() == "application/vnd.openxmlformats-officedocument.wordprocessingml.document") //word (docx)
                    return true;
                if (conentType.ToLower() == "text/plain") //txt
                    return true;
                if (conentType.ToLower() == "application/vnd.ms-excel") //Excel (xls)
                    return true;
                if (conentType.ToLower() == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") //excel (xlst)
                    return true;
                return false;
            }
        }

        public class Query : IRequest<StudentResumeViewModel>
        {
            public int StudentID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, StudentResumeViewModel>
        {

            private readonly ScholarshipForServiceContext _efDB;
            private readonly IStudentRepository _repo;
            public QueryHandler(ScholarshipForServiceContext efDB, IStudentRepository repo) 
            {
                _efDB = efDB; 
                _repo = repo;            
            }


            public async Task<StudentResumeViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var documents = await _efDB.StudentDocuments.Include(m => m.DocumentType)                    
                    .AsNoTracking()
                    .Include(m => m.DocumentType)
                    .Where(m => m.StudentId == request.StudentID && !m.IsDeleted.Value)
                    .Select(m => new
                    {
                        ID = m.StudentDocumentId, 
                        Name = m.FileName,
                        Date = m.DateCreated,
                        Type = m.DocumentType, 
                        FilePath = m.FilePath,
                        IsShareable = m.IsShareable
                    })
                    .ToListAsync();
                 
                StudentResumeViewModel model = new();
                model.ResumeList = new();
                foreach(var resume in documents)
                {
                    if (resume.Type.Code == "Resume")
                    {
                        StudentResumeViewModel.DocumentViewModel r = new StudentResumeViewModel.DocumentViewModel()
                        {
                            DocumentID = resume.ID,
                            Name = resume.Name,
                            Date = resume.Date.HasValue ? resume.Date.Value.ToShortDateString() : "N/A",
                            Type = string.IsNullOrWhiteSpace(resume.FilePath) ? "Builder" :  resume.Type.Code,
                            CanDelete = string.IsNullOrWhiteSpace(resume.FilePath) ? false : true,
                            IsSharable = resume.IsShareable
                        };
                        model.ResumeList.Add(r);
                    }
                }

                model.OtherDocumentCount = documents.Where(m => m.Type.Code is not "Resume").Count();
                var checkForMalicousDocs = await _repo.HandleMalwareScanResults(request.StudentID);
                if (checkForMalicousDocs is not null)
                    model.MalwareResultDocument = checkForMalicousDocs.FileName;
                return model;
            }
        }

        public class QueryDocumentView : IRequest<FileStreamResult>
        {
            public int DocumentId { get; set; }
            public int StudentId { get; set; }
        }

        public class QueryDocumentViewHandler : IRequestHandler<QueryDocumentView, FileStreamResult>
        {
           
            private readonly IDocumentRepository _documentRepo;
			private readonly IAzureBlobService _blobService;
			private readonly IConfiguration _appSettings;

			public QueryDocumentViewHandler(ScholarshipForServiceContext efDB, IDocumentRepository document, IAzureBlobService blobService, IConfiguration appSettings)
            {
                
                _documentRepo = document;
				_blobService = blobService;
				_appSettings = appSettings;
			}

            public async Task<FileStreamResult> Handle(QueryDocumentView request, CancellationToken cancellationToken)
            {
				if (_appSettings["General:Hosting"] == "Macon")
					return await _documentRepo.GetDocumentStreamByDocumentId(request.DocumentId);
				else
				{
					var theDocument = await _documentRepo.GetDocumentByDocumentId(request.DocumentId);
					return await _blobService.DownloadDocumentStreamAsync(theDocument.FilePath, theDocument.FileName);

				}
			}
        }

        public class CommandDelete : IRequest<bool>
        {
            public int DocumentID { get; set; }
            public int StudentID { get; set; }
        }

        public class CommandDeleteHandler : IRequestHandler<CommandDelete, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public CommandDeleteHandler(ScholarshipForServiceContext efDB) => _efDB = efDB;

            public async Task<bool> Handle(CommandDelete request, CancellationToken cancellationToken)
            {
                var docToDelete = await _efDB.StudentDocuments.Where(m => m.StudentDocumentId == request.DocumentID && m.StudentId == request.StudentID).FirstOrDefaultAsync();
                if(docToDelete != null)
                {
                    docToDelete.IsDeleted = true;
                    await _efDB.SaveChangesAsync();
                }
                else
                {
                    //TODO add logging for when a delete attempt on a document that doesn't match the docID & studentID value....
                    return false;
                }
                return true;
            }
        }

        public class CommandUpload : IRequest<CommandUploadHandler.CommandUploadResult>
        {
            public int StudentID { get; set; }
            public string Name { get; set; }
            public IFormFile UploadedResume { get; set; }
        }
        public class CommandUploadHandler : IRequestHandler<CommandUpload, CommandUploadHandler.CommandUploadResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IAntiVirusHelper _avService;
			private readonly IAzureBlobService _blobService;
			private readonly IConfiguration _appSettings;


			public CommandUploadHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IAntiVirusHelper avService, IAzureBlobService blobService, IConfiguration appSettings)
            {
                _cache = cache;
                _efDB = efDB;
                _avService = avService;
				_blobService = blobService;
				_appSettings = appSettings;
			}

            public async Task<CommandUploadResult> Handle(CommandUpload request, CancellationToken cancellationToken)
            {
				//Save to file share
				var fileName = $"{request.Name}{System.IO.Path.GetExtension(request.UploadedResume.FileName)}";
				var fullPath = "";
				var docType = await _efDB.DocumentTypes.AsNoTracking()
							.Where(m => m.Code == "Resume")
							.Select(m => new
							{
								ID = m.DocumentTypeId
							})
							.FirstOrDefaultAsync();

				if (request.UploadedResume.Length > 0)
                {
                    if (_appSettings["General:Hosting"] == "Macon")
                    {
                        var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                        var fileShare = GlobalConfigSettings.Where(m => m.Key == "Fileserver" && m.Type == "Document").Select(m => m.Value).FirstOrDefault();
                        var AVServer = GlobalConfigSettings.Where(m => m.Key == "AVScanner").Select(m => m.Value).FirstOrDefault();
                        using (Stream fileStream = request.UploadedResume.OpenReadStream())
                        {
                            var isFileAVClean = _avService.IsDocAVClean(fileStream, AVServer, request.StudentID.ToString());
                            if (!isFileAVClean) return new CommandUploadResult() { IsSuccess = false, ErrorMessage = "There is a problem uploading your document. Please try again later." };
                        }

                        
                        var scrambledName = $"{Guid.NewGuid().ToString()}{System.IO.Path.GetExtension(request.UploadedResume.FileName)}";
                        fullPath = GetPath(fileShare, scrambledName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await request.UploadedResume.CopyToAsync(stream);
                        }
                        
                    }
					else
					{
						fullPath = await _blobService.UploadDocumentStreamAsync(request.UploadedResume, request.StudentID, "ST");
					}
					//Save to DB
					StudentDocument newResume = new StudentDocument()
                    {
                        StudentId = request.StudentID,
                        DateCreated = DateTime.UtcNow,
                        FileName = fileName,
                        FilePath = fullPath,
                        IsDeleted = false,
                        DocumentTypeId = docType.ID,
                        UserId = request.StudentID.ToString()
                    };

                    await _efDB.StudentDocuments.AddAsync(newResume);
                    await _efDB.SaveChangesAsync();
                    return new CommandUploadResult() { IsSuccess = true};
                }

                return new CommandUploadResult() { IsSuccess = false, ErrorMessage = "An unexpected error has occured" };
            }

            public class CommandUploadResult
            {
                public bool IsSuccess { get; set; }
                public string ErrorMessage { get; set; }
            }

            private string GetPath(string testShare, string fileName)
            {
                Random rand = new Random();
                int subfolder = rand.Next(0, 20);
                var filePath = string.Format(@"{0}\{1}", testShare, subfolder);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                return System.IO.Path.Combine(filePath, fileName);
            }
        }

        public class CommandShareResume : IRequest<bool>
        {
            public int StudentID { get; set; }
            public int DocumentID { get; set; }
        }

        public class CommandShareResumeHandler : IRequestHandler<CommandShareResume, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public CommandShareResumeHandler(ScholarshipForServiceContext efDB) => _efDB = efDB;

            public async Task<bool> Handle(CommandShareResume request, CancellationToken cancellationToken)
            {
                var currentSharableResume = await _efDB.StudentDocuments.Where(m => m.IsShareable == true && m.StudentId == request.StudentID).FirstOrDefaultAsync();
                var resumeToShare = await _efDB.StudentDocuments.Where(m => m.StudentDocumentId == request.DocumentID && m.StudentId == request.StudentID).FirstOrDefaultAsync();
                if (resumeToShare != null)
                {
                    if(currentSharableResume != null)
                    {
                        currentSharableResume.IsShareable = false;
                    }
                    resumeToShare.IsShareable  = true;
                    await _efDB.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }
    }
}
