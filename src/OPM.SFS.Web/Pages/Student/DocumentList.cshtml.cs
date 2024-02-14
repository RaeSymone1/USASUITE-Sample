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
    public class DocumentListModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public OtherDocumentViewModel Data { get; set; }

        [FromQuery(Name = "fid")]
        public int DocumentId { get; set; } = 0;

        public DocumentListModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentID = User.GetUserId() });
        }

        public async Task<IActionResult> OnPostUploadAsync(UploadDocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                return new ObjectResult(new { status = error.ErrorMessage });

            }
            var result = await _mediator.Send(new CommandUpload() { UploadedDocument = model.Document, 
                StudentID = User.GetUserId(), 
                Name = model.Name, DocumentType = model.DocumentType });
            if(result.IsSuccess) return new ObjectResult(new { status = "success" });
            return new ObjectResult(new { status = result.ErrorMessage });


        }

        public async Task<FileResult> OnGetViewDocument()
        {
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = DocumentId, StudentId = User.GetUserId() });
            return file;
        }

        public async Task<ActionResult> OnPostDeleteAsync(string docid)
        {
            var result = await _mediator.Send(new CommandDelete()
            {
                DocumentID = Convert.ToInt32(docid),
                StudentID = User.GetUserId()
            });
            return Redirect($"/Student/DocumentList");

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

        public class Query : IRequest<OtherDocumentViewModel>
        {
            public int StudentID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, OtherDocumentViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IStudentRepository _repo;

            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IStudentRepository repo)
            {
                _efDB = efDB;
                _cache = cache;
                _repo = repo;
            }


            public async Task<OtherDocumentViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var documents = await _efDB.StudentDocuments.Include(m => m.DocumentType)
                   .AsNoTracking()
                   .Where(m => m.StudentId == request.StudentID && !m.IsDeleted.Value)
                   .Select(m => new
                   {
                       ID = m.StudentDocumentId,
                       Name = m.FileName,
                       Date = m.DateCreated,
                       Type = m.DocumentType
                   })
                   .ToListAsync();

                OtherDocumentViewModel model = new();
                model.DocumentList = new();
                foreach (var doc in documents)
                {
                    if (doc.Type.Code is not "Resume")
                    {
                        OtherDocumentViewModel.DocumentViewModel r = new OtherDocumentViewModel.DocumentViewModel()
                        {
                            DocumentID = doc.ID,
                            Name = doc.Name,
                            Date = doc.Date.HasValue ? doc.Date.Value.ToShortDateString() : "N/A",
                            Type = doc.Type.Code,
                            CanDelete = true
                        };
                        model.DocumentList.Add(r);
                    }
                }
                model.ResumeCount = documents.Where(m => m.Type.Code == "Resume").Count();
                var validDocTypes = await _cache.GetDocumentTypeAsync();
                model.DocTypes = new();
                foreach (var item in validDocTypes)
                {
                    if (!item.IsDisabled && item.Code is not "SF50" && item.Code is not "EVF")
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
            public int StudentId { get; set; }
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
        }
        public class CommandUploadHandler : IRequestHandler<CommandUpload, CommandUploadHandler.CommandUploadResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IAntiVirusHelper _avService;
            private readonly ICommitmentNotificationService _emailer;
			private readonly IAzureBlobService _blobService;
			private readonly IConfiguration _appSettings;
            private readonly ILogger<CommandHandler> _logger;

            public CommandUploadHandler(ScholarshipForServiceContext efDB, ICommitmentNotificationService emailer, ICacheHelper cache, IAntiVirusHelper avService, IAzureBlobService blobService, IConfiguration appSettings, ILogger<CommandHandler> logger)
            {
                _cache = cache;
                _efDB = efDB;
                _emailer = emailer;
                _avService = avService;
				_blobService = blobService;
                _logger = logger;
				_appSettings = appSettings;
			}


            public async Task<CommandUploadResult> Handle(CommandUpload request, CancellationToken cancellationToken)
            {
				//Save to file share
				var fileName = $"{request.Name}{System.IO.Path.GetExtension(request.UploadedDocument.FileName)}";
				var fullPath = "";
				if (request.UploadedDocument.Length > 0)
                {
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
                        fullPath = await _blobService.UploadDocumentStreamAsync(request.UploadedDocument, request.StudentID, "ST");
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

                    var pgCommitments = await _efDB.StudentCommitments
                     .Where(m => m.StudentId == request.StudentID).Include(m => m.SupervisorContact)
                     .Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                     .Include(m => m.Agency).ThenInclude(m => m.AgencyType)
                     .Include(m => m.CommitmentType).Where(m => m.CommitmentType.Name == "PostGraduate")
                     .Include(m => m.CommitmentStatus)
                     .ToListAsync();

                    var docTypes = await _cache.GetDocumentTypeAsync();
                    var SF50 = docTypes.Where(m => m.Name == "SF-50").Select(m => m.Name).FirstOrDefault();
                    var EVF = docTypes.Where(m => m.Name == "Employment Verification Form").Select(m => m.Name).FirstOrDefault();
                    var selectedDocTypeName = docTypes.Where(m => m.DocumentTypeId == request.DocumentType).Select(m => m.Name).FirstOrDefault();
                    List<CommitmentNotificationRequest> emailData = new List<CommitmentNotificationRequest>();
                    int counter = 1;
                    foreach (var commitment in pgCommitments) {
                        var item = new CommitmentNotificationRequest
                        {
                            Commitment = commitment,
                            InstitutionID = commitment.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value
                        };
                        emailData.Add(item);
                    counter++;
                    };

                    if ((selectedDocTypeName == SF50)||(selectedDocTypeName == EVF))
                        await _emailer.SendEmailWhenStudentUploadEVFDocument(emailData);
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
                if (docToDelete != null)
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

    }
}
