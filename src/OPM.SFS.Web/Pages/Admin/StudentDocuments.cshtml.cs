using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class StudentDocumentsModel : PageModel
    {
        [BindProperty]
        public AdminStudentDocumentViewModel Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; }

        [FromQuery(Name = "fid")]
        public int DocumentId { get; set; }

        private readonly IMediator _mediator;
        public StudentDocumentsModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentID = StudentID });

        }
        public async Task<IActionResult> OnPostUploadAsync(UploadDocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                return new ObjectResult(new { status = error.ErrorMessage });

            }
            var result = await _mediator.Send(new CommandUpload()
            {
                UploadedDocument = model.Document,
                StudentID = model.StudentID,
                Name = model.Name,
                DocumentType = model.DocumentType,
                UserID = User.FindFirstValue("SFS_UserID"),
                UserType = "AD"

            });
            if (result.IsSuccess) return new ObjectResult(new { status = "success" });
            return new ObjectResult(new { status = result.ErrorMessage });


        }
        public async Task<FileResult> OnGetViewDocument()
        {
            //https://www.jerriepelser.com/blog/razor-pages-muliple-handlers/
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = DocumentId });
            return file;
        }

        public async Task<IActionResult> OnGetDeleteDocumentAsync()
        {
            _ = await _mediator.Send(new CommandDocumentDelete() { DocumentID = DocumentId, StudentID = StudentID });
            return RedirectToPage("/Admin/StudentDocuments", new { sid = StudentID });
        }
        public class DocumentUploadValidator : AbstractValidator<UploadDocumentViewModel>
        {
            public DocumentUploadValidator()
            {
                RuleFor(x => x.Name).Matches("^[^><&]+$");
                RuleFor(x => x.Document).NotNull().WithMessage("Resume is required.");
                When(x => x.Document != null, () =>
                {
                    RuleFor(x => x.Document.Length).NotNull().LessThanOrEqualTo(5242880)
                        .WithMessage("File size exceeds 5MB");
                    RuleFor(x => x.Document.ContentType).Must((o, positiondescription) => { return IsValidDocType(o.Document.ContentType); })
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

        public class Query : IRequest<AdminStudentDocumentViewModel>
        {
            public int StudentID { get; set;}
        }

        public class QueryHandler : IRequestHandler<Query, AdminStudentDocumentViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;
            private readonly IStudentRepository _repo;


            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache, IStudentRepository repo)
            {
                _db = db;
                _cache = cache;
                _repo = repo;
            }
            public async Task<AdminStudentDocumentViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AdminStudentDocumentViewModel model = new();
                model.Documents = new();
                model.StudentID = request.StudentID;

                var studentData = await _db.Students.Where(m => m.StudentId == request.StudentID)
                    .Select(m => new
                    {
                        StudentName = $"{m.FirstName} {m.LastName}",
                        Instition = m.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                        Degree = m.StudentInstitutionFundings.FirstOrDefault().Degree.Name,
                        Major = m.StudentInstitutionFundings.FirstOrDefault().Major.Name,
                        GradDate = m.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate
                    })
                    .FirstOrDefaultAsync();

                model.Studentname = studentData.StudentName;
                model.University = studentData.Instition;
                model.Major = studentData.Major;
                model.Degree = studentData.Degree;
                model.GradDate = studentData.GradDate.HasValue ? $"{studentData.GradDate.Value.Month}/{studentData.GradDate.Value.Year}" : "";

                var documents = await _db.StudentDocuments
                    .Where(m => m.StudentId == request.StudentID && m.IsDeleted == false)
                    .Select(m => new
                    {
                        ID = m.StudentDocumentId,
                        Name = m.FileName,
                        Type = m.DocumentType.Name,
                        UploadDate = m.DateCreated,
                        FilePath = m.FilePath
                       
                    })
                   .ToListAsync();                             

                foreach (var d in documents)
                {
                    AdminStudentDocumentViewModel.DocumentItem doc = new();
                    doc.DocumentName = d.Name;
                    doc.DocumentType = d.Type;
                    doc.DocumentID = d.ID;
                    doc.DateUploaded = d.UploadDate.HasValue ? d.UploadDate.Value.ToShortDateString() : "N/A";
                    doc.IsBuilderResume = string.IsNullOrWhiteSpace(d.FilePath) ? true : false;
                    model.Documents.Add(doc);
                }
                var validDocTypes = await _cache.GetDocumentTypeAsync();
                model.DocTypes = new();
                foreach (var item in validDocTypes)
                {
                    if (!item.IsDisabled)
                    {
                        model.DocTypes.Add(new DocumentTypeViewModel()
                        {
                            Code = item.Code,
                            DocumentTypeID = item.DocumentTypeId,
                            DocumentTypeDispley = item.Name
                        });
                    }
                }
                var checkForMalicousDocs = await _repo.HandleMalwareScanResults(request.StudentID);
                if (checkForMalicousDocs is not null)
                    model.MalwareResultDocument = checkForMalicousDocs.FileName;
                return model;
            }
        }

        public class QueryDocumentView : IRequest<FileStreamResult>
        {
            public int DocumentId { get; set; }
        }

        public class QueryDocumentViewHandler : IRequestHandler<QueryDocumentView, FileStreamResult>
        {
            private readonly IDocumentRepository _documentRepo;
			private readonly IAzureBlobService _blobService;
			private readonly IConfiguration _appSettings;
            private readonly ILogger<QueryDocumentViewHandler> _logger;

            public QueryDocumentViewHandler(IDocumentRepository document, IAzureBlobService blobService, IConfiguration appSettings, ILogger<QueryDocumentViewHandler> logger)
			{
				_documentRepo = document;
				_blobService = blobService;
				_appSettings = appSettings;
                _logger = logger;
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
        public class CommandUpload : IRequest<CommandUploadHandler.CommandUploadResult>
        {
            public int StudentID { get; set; }
            public string Name { get; set; }
            public int DocumentType { get; set; }
            public IFormFile UploadedDocument { get; set; }
            public AdminStudentDocumentViewModel Model { get; set; }
            public string UserID { get; set; }
            public string UserType { get; set; }
        }
        public class CommandUploadHandler : IRequestHandler<CommandUpload, CommandUploadHandler.CommandUploadResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IAntiVirusHelper _avService;
			private readonly IConfiguration _appSettings;
            private readonly IAzureBlobService _blobService;
            private readonly ILogger<CommandUploadHandler> _logger;

            public CommandUploadHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IAntiVirusHelper avService, IConfiguration appSettings, IAzureBlobService blobservice, ILogger<CommandUploadHandler> logger)
            {
                _cache = cache;
                _efDB = efDB;
                _avService = avService;
                _appSettings = appSettings;
                _blobService = blobservice;
                _logger = logger;
            }


            public async Task<CommandUploadResult> Handle(CommandUpload request, CancellationToken cancellationToken)
            {
                //Save to file share
                if (request.UploadedDocument.Length > 0)
                {
					var fileName = $"{request.Name}{System.IO.Path.GetExtension(request.UploadedDocument.FileName)}";
                    var fullPath = "";
                    //Adding toggle for Azure migration testing
					if (_appSettings["General:Hosting"] == "Macon")
                    {
						var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
						var fileShare = GlobalConfigSettings.Where(m => m.Key == "Fileserver" && m.Type == "Document").Select(m => m.Value).FirstOrDefault();
						var AVServer = GlobalConfigSettings.Where(m => m.Key == "AVScanner").Select(m => m.Value).FirstOrDefault();
						using (Stream fileStream = request.UploadedDocument.OpenReadStream())
						{
							var isFileAVClean = _avService.IsDocAVClean(fileStream, AVServer, request.StudentID.ToString());
							if (!isFileAVClean) return new CommandUploadResult() { IsSuccess = false, ErrorMessage = "There is a problem uploading your document. Please try again later." };
						}
						
						var scrambledName = $"{Guid.NewGuid().ToString()}{System.IO.Path.GetExtension(request.UploadedDocument.FileName)}";
						fullPath = GetPath(fileShare, scrambledName);

						using (var stream = new FileStream(fullPath, FileMode.Create))
						{
							await request.UploadedDocument.CopyToAsync(stream);
						}
					}
                    else
                    {						
						fullPath = await _blobService.UploadDocumentStreamAsync(request.UploadedDocument, Convert.ToInt32(request.UserID), request.UserType);
                        _logger.LogInformation($"Document upload to blob storage - {fullPath}");
                    }
                    
                    //Save to DB
                    StudentDocument newResume = new StudentDocument()
                    {
                        StudentId = request.StudentID,
                        DateCreated = DateTime.UtcNow,
                        FileName = fileName,
                        FilePath = fullPath,
                        IsDeleted = false,
                        DocumentTypeId = request.DocumentType,
                        UserId = request.StudentID.ToString()

                    };

                    await _efDB.StudentDocuments.AddAsync(newResume);
                    await _efDB.SaveChangesAsync();
                    return new CommandUploadResult() { IsSuccess = true };
                }

                return new CommandUploadResult() { IsSuccess = false, ErrorMessage = "There is a problem uploading your document. Please try again later." };
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
        public class CommandDocumentDelete : IRequest<bool>
        {           
            public int StudentID { get; set; }
            public int DocumentID { get; set; }
        }

        public class CommandDocumentDeleteHandler : IRequestHandler<CommandDocumentDelete, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            public CommandDocumentDeleteHandler(ScholarshipForServiceContext db)
            {
                _db = db;
            }
            public async Task<bool> Handle(CommandDocumentDelete request, CancellationToken cancellationToken)
            {
                var docToDelete = await _db.StudentDocuments.Where(m => m.StudentDocumentId == request.DocumentID && m.StudentId == request.StudentID).FirstOrDefaultAsync();
                if (docToDelete is not null)
                {
                    docToDelete.IsDeleted = true;
                    await _db.SaveChangesAsync();
                }

                return true;
            }
        }
    }
}

