using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class CommitmentDocumentsModel : PageModel
    {
        [FromQuery(Name = "cid")]
        public int CommitId { get; set; } = 0;
        [FromQuery(Name = "fid")]
        public int DocumentId { get; set; } = 0;

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;

     
        [BindProperty]
        public CommitmentDocumentViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public CommitmentDocumentsModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudId = StudentID, CommitId = CommitId });

        }

        public async Task<FileResult> OnGetViewDocument()
        {           
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = DocumentId });
            return file;                       
        }

        public async Task<IActionResult> OnPostSubmitCommitmentAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var hasUpdated = await _mediator.Send(new SubmitCommand() { Model = Data  });
            if (hasUpdated)
                return RedirectToPage("/Admin/CommitmentDocuments", new { cid = Data.CommitmentID });
            return Page();
        }

        public async Task<IActionResult> OnPostUploadPosDescriptionAsync()
        {
            if (!ModelState.IsValid) return Page();
            var submit = await _mediator.Send(new Command() { Model = Data, DocumentType = "PositionDesc", UserID = User.FindFirstValue("SFS_UserID"), UserType = "AD" });
            if(submit.IsSuccessful) return RedirectToPage("/Admin/CommitmentDocuments", new { cid = Data.CommitmentID });
            ModelState.AddModelError("", submit.ErrorMessage);
            return Page();
        }

        public async Task<IActionResult> OnPostUploadTenativeJobOfferAsync()
        {
            if (!ModelState.IsValid) return Page();
            var submit = await _mediator.Send(new Command() { Model = Data, DocumentType = "TentativeOffer", UserID = User.FindFirstValue("SFS_UserID"), UserType = "AD" });
            if(submit.IsSuccessful) return RedirectToPage("/Admin/CommitmentDocuments", new { cid = Data.CommitmentID });
            ModelState.AddModelError("", submit.ErrorMessage);
            return Page();
        }

        public async Task<IActionResult> OnPostUploadFinalJobOfferAsync()
        {
            if (!ModelState.IsValid) return Page();
            var submit = await _mediator.Send(new Command() { Model = Data, DocumentType = "FinalOffer", UserID = User.FindFirstValue("SFS_UserID"), UserType = "AD" });
            if(submit.IsSuccessful) return RedirectToPage("/Admin/CommitmentDocuments", new { cid = Data.CommitmentID });
            ModelState.AddModelError("", submit.ErrorMessage);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteDocumentAsync()
        {           
            _ = await _mediator.Send(new CommandDocumentDelete() {  CommitmentDocumentId = DocumentId });
            return RedirectToPage("/Admin/CommitmentDocuments", new { cid = CommitId });
        }

        public class CommitmentDocumentModelValidator : AbstractValidator<CommitmentDocumentViewModel>
        {
           
            public CommitmentDocumentModelValidator()
            {

                When(x => x.PostionDescription != null, () =>
                {
                    RuleFor(x => x.PostionDescription.Length).NotNull().LessThanOrEqualTo(5242880)
                    .WithMessage("File size exceeds 5MB");
                    RuleFor(x => x.PostionDescription.ContentType).Must((o, positiondescription) => { return IsValidDocType(o.PostionDescription.ContentType); })
                    .WithMessage("Files must be one of the following formats: TXT, PDF, Word (DOC or DOCX) or Excel (XLS, XSLX)"); ;
                });

                When(x => x.FinalJobOffer != null, () =>
                {
                    RuleFor(x => x.FinalJobOffer.Length).NotNull().LessThanOrEqualTo(5242880)
                    .WithMessage("File size exceeds 5MB");
                    RuleFor(x => x.FinalJobOffer.ContentType).Must((o, positiondescription) => { return IsValidDocType(o.FinalJobOffer.ContentType); })
                    .WithMessage("Files must be one of the following formats: TXT, PDF, Word (DOC or DOCX) or Excel (XLS, XSLX)"); ;
                });

                When(x => x.TenativeJobOffer != null, () =>
                {
                    RuleFor(x => x.TenativeJobOffer.Length).NotNull().LessThanOrEqualTo(5242880)
                    .WithMessage("File size exceeds 5MB");
                    RuleFor(x => x.TenativeJobOffer.ContentType).Must((o, positiondescription) => { return IsValidDocType(o.TenativeJobOffer.ContentType); })
                        .WithMessage("Files must be one of the following formats: TXT, PDF, Word (DOC or DOCX) or Excel (XLS, XSLX)");
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


        public class Query : IRequest<CommitmentDocumentViewModel>
        {
            public int StudId { get; set; }
            public int CommitId { get; set; }
            public string StatusMessage { get; set; }

        }

        public class QueryHandler : IRequestHandler<Query, CommitmentDocumentViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public QueryHandler(ScholarshipForServiceContext efDB)
            {
                _efDB = efDB;
            }
            public async Task<CommitmentDocumentViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                CommitmentDocumentViewModel model = new CommitmentDocumentViewModel();
                model.CommitmentID = request.CommitId;
                var lookupData = await _efDB.StudentCommitments.AsNoTracking().Where(m => m.StudentCommitmentId == request.CommitId)
                    .Select(b => new 
                    {
                        Status = b.CommitmentStatus.Value, 
                        StatusCode = b.CommitmentStatus.Code,
                        ApprovalFlow = b.Agency.CommitmentApprovalWorkflow.Code, 
                        StudentID = b.StudentId})
                    .FirstOrDefaultAsync();

                if (lookupData.StatusCode == CommitmentProcessConst.RequestFinalDocs && lookupData.ApprovalFlow == CommitmentProcessConst.CommitmentApprovalTentative)
                {
                    model.ApprovalFlow = CommitmentProcessConst.CommitmentApprovalFinal;
                }
                else if (lookupData.ApprovalFlow == CommitmentProcessConst.CommitmentApprovalTentative && lookupData.StatusCode == CommitmentProcessConst.Approved )
                {
                    model.ApprovalFlow = CommitmentProcessConst.CommitmentApprovalFinal;
                }
                else
                {
                    model.ApprovalFlow = lookupData.ApprovalFlow;
                }                
                model.Status = lookupData.Status;
                
                model.StudentID = lookupData.StudentID;
                if (request.CommitId > 0)
                {
                    var docs = await _efDB.CommitmentStudentDocument
                        .Where(m => m.StudentCommitmentID == request.CommitId && m.StudentDocument.IsDeleted != true)
                        .Select(m => new
                        {
                            DocumentTypeCode = m.StudentDocument.DocumentType.Code,
                            StudentDocID = m.StudentDocumentID,
                            FileName = m.StudentDocument.FileName,
                            DocumentTypeName = m.StudentDocument.DocumentType.Name
                        })
                        .ToListAsync();
                    model.SavedDocuments = new List<CommitmentDocumentViewModel.SavedDocument>();
                    foreach (var doc in docs)
                    {
                        if (doc.DocumentTypeCode == "TentativeOffer")
                        {
                            model.HideUploadForTenative = true;
                        }

                        if (doc.DocumentTypeCode == "FinalOffer")
                        {
                            model.HideUploadForFinalJobLetter = true;
                        }
                        if (doc.DocumentTypeCode == "PositionDesc")
                        {
                            model.HideUploadForPositionDescription = true;
                        }
                        model.SavedDocuments.Add(new CommitmentDocumentViewModel.SavedDocument()
                        {
                            Id = doc.StudentDocID,
                            Name = doc.FileName.Contains('_') ? doc.FileName.Substring(doc.FileName.IndexOf("_") + 1) : doc.FileName,
                            Type = doc.DocumentTypeName
                        });
                    }
                    model.SavedDocumentCount = docs.Count();
                }
               
                return model;
            }
        }

        public class Command : IRequest<CommandHandler.UploadCommandResult>
        {
            public CommitmentDocumentViewModel Model { get; set; }
            public string DocumentType { get; set; }
            public string UserID { get; set; }
            public string UserType { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandHandler.UploadCommandResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IAntiVirusHelper _avService;
            private readonly IStudentDashboardService _service;
            private readonly IAzureBlobService _blobService;
            private readonly ILogger<CommandHandler> _logger;
            private readonly IConfiguration _appSettings;

            public CommandHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IAntiVirusHelper avService, IStudentDashboardService service, IAzureBlobService blobService, ILogger<CommandHandler> logger, IConfiguration appSettings)
            {
                _efDB = efDB;
                _cache = cache;
                _avService = avService;
                _service = service;
                _blobService = blobService;
                _logger = logger;
                _appSettings = appSettings;
            }

            public async Task<CommandHandler.UploadCommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                IFormFile docToUpload = null;
                if (request.DocumentType == "PositionDesc")
                    docToUpload = request.Model.PostionDescription;
                if (request.DocumentType == "TentativeOffer")
                    docToUpload = request.Model.TenativeJobOffer;
                if (request.DocumentType == "FinalOffer")
                    docToUpload = request.Model.FinalJobOffer;
                var fullPath = "";
                if (docToUpload.Length > 0)
                {
                    if (_appSettings["General:Hosting"] == "Macon")
                    {
                        var fileShare = GlobalConfigSettings.Where(m => m.Key == "Fileserver" && m.Type == "Document").Select(m => m.Value).FirstOrDefault();
                        var AVServer = GlobalConfigSettings.Where(m => m.Key == "AVScanner").Select(m => m.Value).FirstOrDefault();
                        using (Stream fileStream = docToUpload.OpenReadStream())
                        {
                            var isFileAVClean = _avService.IsDocAVClean(fileStream, AVServer, request.Model.StudentID.ToString());
                            if (!isFileAVClean) return new UploadCommandResult() { IsSuccessful = false, CommitmentID = request.Model.CommitmentID, ErrorMessage = "There is a problem uploading your document. Please try again later." };
                        }
                        var fileName = $"{Guid.NewGuid().ToString()}_{docToUpload.FileName}";
                        fullPath = GetPath(fileShare, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await docToUpload.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        
                        fullPath = await _blobService.UploadDocumentStreamAsync(docToUpload, Convert.ToInt32(request.UserID), request.UserType);
                        _logger.LogInformation($"Document upload to blob storage - {fullPath}");
                    }
                   

                    using var transaction = _efDB.Database.BeginTransaction();
                    var student = _efDB.Students.Where(m => m.StudentId == request.Model.StudentID).FirstOrDefault();
                    var docExtension = System.IO.Path.GetExtension(docToUpload.FileName);
                    StudentDocument docToAdd = new();
                    docToAdd.FileName = docToUpload.FileName.Replace(docExtension, "");
                    docToAdd.FilePath = fullPath;
                    docToAdd.DocumentTypeId = _efDB.DocumentTypes.Where(m => m.Code.ToUpper() == request.DocumentType.ToUpper()).Select(m => m.DocumentTypeId).FirstOrDefault();
                    docToAdd.DateCreated = DateTime.UtcNow;
                    student.StudentDocuments.Add(docToAdd);
                    await _efDB.SaveChangesAsync();

                    //link document to the Commitment
                    var commitmentRecord = _efDB.StudentCommitments
                        .Where(m => m.StudentId == request.Model.StudentID && m.StudentCommitmentId == request.Model.CommitmentID)
                        .Include(m => m.CommitmentType)
                        .FirstOrDefault();
                    commitmentRecord.CommitmentStudentDocuments.Add(new CommitmentStudentDocument()
                    {
                        StudentDocumentID = docToAdd.StudentDocumentId,
                        DateAdded = DateTime.UtcNow
                    });

                    //if this is a commitment submitted by the admins update the status
                    var allStatus = await _cache.GetCommitmentStatusAsync();
                    var incompleteID = allStatus.Where(m => m.Code == CommitmentProcessConst.Incomplete).Select(m => m.CommitmentStatusID).FirstOrDefault();
                    if (commitmentRecord.CommitmentStatusId.Value == incompleteID)
                    {
                        var pendingPO = allStatus.Where(m => m.Code == CommitmentProcessConst.ApprovalPendingPO).Select(m => m.CommitmentStatusID).FirstOrDefault();
                        commitmentRecord.DateSubmitted = DateTime.UtcNow;
                        commitmentRecord.CommitmentStatusId = pendingPO;
                        

                    }
                    await _efDB.SaveChangesAsync();
                    await transaction.CommitAsync();
                    await _service.UpdateCommitmentReportedForDashboard(request.Model.StudentID);
				}
                return new UploadCommandResult() { CommitmentID = request.Model.CommitmentID, IsSuccessful = true };
            }

            public class UploadCommandResult
            {
                public int CommitmentID { get; set; }
                public bool IsSuccessful { get; set; }
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

        public class SubmitCommand : IRequest<bool>
        {
            public CommitmentDocumentViewModel Model { get; set; }
        }

        public class SubmitCommandHandler : IRequestHandler<SubmitCommand, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IEmailerService _emailer;
            private readonly ICacheHelper _cache;
          
            public SubmitCommandHandler(ScholarshipForServiceContext efDB, IEmailerService emailer, ICacheHelper cache)
            {
                _efDB = efDB;
                _emailer = emailer;
                _cache = cache;               
            }

            public async Task<bool> Handle(SubmitCommand request, CancellationToken cancellationToken)
            {                

                var commitmentStatusList = await _cache.GetCommitmentStatusAsync();
               var commitmentToUpdate =  await _efDB.StudentCommitments
                    .Where(m => m.StudentCommitmentId == request.Model.CommitmentID).FirstOrDefaultAsync();
                if(request.Model.ApprovalFlow == "APPR")
                {
                    commitmentToUpdate.CommitmentStatusId = commitmentStatusList.Where(m => m.Value == "POApprovalPending").Select(m => m.CommitmentStatusID).FirstOrDefault();
                }
                else
                {                   
                    commitmentToUpdate.CommitmentStatusId = commitmentStatusList.Where(m => m.Value == "PIApprovalPending").Select(m => m.CommitmentStatusID).FirstOrDefault();
                }
                
                commitmentToUpdate.LastModified = DateTime.UtcNow;
                await _efDB.SaveChangesAsync();
                return true;
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
                    return await _documentRepo.GetDocumentStreamByDocumentId(request.DocumentId);
                }
                else
                {
                    var theDocument = await _documentRepo.GetDocumentByDocumentId(request.DocumentId);
                    return await _blobService.DownloadDocumentStreamAsync(theDocument.FilePath, theDocument.FileName);
                }
            }
        }
        public class CommandDocumentDelete : IRequest<bool>
        {
            public int CommitmentDocumentId { get; set; }
            public int StudentId { get; set; }
        }

        public class CommandDocumentDeleteHandler : IRequestHandler<CommandDocumentDelete, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public CommandDocumentDeleteHandler(ScholarshipForServiceContext efDB)
            {
                _efDB = efDB;
            }
            public async Task<bool> Handle(CommandDocumentDelete request, CancellationToken cancellationToken)
            {
                var docToDelete = await _efDB.StudentDocuments.Where(m => m.StudentDocumentId == request.CommitmentDocumentId).FirstOrDefaultAsync();
                if(docToDelete is not null)
                {
                    docToDelete.IsDeleted = true;
                    await _efDB.SaveChangesAsync();
                }
                
                return true;
            }
        }
    }

  

    
}
