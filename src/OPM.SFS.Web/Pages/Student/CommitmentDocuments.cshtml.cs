using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
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
using OPM.SFS.Web.Handlers;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using static OPM.SFS.Web.SharedCode.CommitmentProcessService;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    [IsProfileCompleteFilter()]
    public class CommitmentDocumentsModel : PageModel
    {
        [FromQuery(Name = "cid")]
        public int CommitId { get; set; } = 0;
        [FromQuery(Name = "fid")]
        public int DocumentId { get; set; } = 0;

        [BindProperty]
        public CommitmentDocumentViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public CommitmentDocumentsModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudId = User.GetUserId(), CommitId = CommitId });

        }

        public async Task<FileResult> OnGetViewDocument()
        {
            //https://www.jerriepelser.com/blog/razor-pages-muliple-handlers/
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = DocumentId });
            return file;                       
        }

        public async Task<IActionResult> OnPostSubmitCommitmentAsync()
        {
            var EVFToggle = Convert.ToBoolean(User.FindFirst("EVFToggle").Value);

            if (!ModelState.IsValid)
            {
                return Page();
            }            

            var hasUpdated = await _mediator.Send(new SubmitCommand() { Model = Data });
            if (EVFToggle)
            {
                if (hasUpdated)
                    return RedirectToPage("/Student/CommitmentVerification");
            }
            else
            {
                if (hasUpdated)
                    return RedirectToPage("/Student/Commitments");
            }
            if (hasUpdated)
                return RedirectToPage("/Student/Commitments");  
            return Page();
        }

        public async Task<IActionResult> OnPostUploadPosDescriptionAsync()
        {
            if (!ModelState.IsValid) return Page();

            var validator = new CommitmentDocumentValidator();
            var results = validator.IsValidDocumentForUpload(Data.PostionDescription);
            if (!results.IsSuccess)
            {
                ModelState.AddModelError("", results.Message);
                return Page();
            }

            var submit = await _mediator.Send(new Command() { Model = Data, StudentId = User.GetUserId(), DocumentType = "PositionDesc" });
            if(submit.IsSuccessful) return RedirectToPage("/Student/CommitmentDocuments", new { cid = Data.CommitmentID });
            ModelState.AddModelError("", submit.ErrorMessage);
            return Page();
        }

        public async Task<IActionResult> OnPostUploadTenativeJobOfferAsync()
        {
            if (!ModelState.IsValid) return Page();
            var validator = new CommitmentDocumentValidator();
            var results = validator.IsValidDocumentForUpload(Data.TenativeJobOffer);
            if (!results.IsSuccess)
            {                
                ModelState.AddModelError("", results.Message);
                return Page();
            }
            var submit = await _mediator.Send(new Command() { Model = Data, StudentId = User.GetUserId(), DocumentType = "TentativeOffer" });
            if(submit.IsSuccessful) return RedirectToPage("/Student/CommitmentDocuments", new { cid = Data.CommitmentID });
            ModelState.AddModelError("", submit.ErrorMessage);
            return Page();
        }

        public async Task<IActionResult> OnPostUploadFinalJobOfferAsync()
        {
            if (!ModelState.IsValid) return Page();
            var validator = new CommitmentDocumentValidator();
            var results = validator.IsValidDocumentForUpload(Data.FinalJobOffer);
            if (!results.IsSuccess)
            {
                ModelState.AddModelError("", results.Message);
                return Page();
            }
            var submit = await _mediator.Send(new Command() { Model = Data, StudentId = User.GetUserId(), DocumentType = "FinalOffer" });
            if(submit.IsSuccessful) return RedirectToPage("/Student/CommitmentDocuments", new { cid = Data.CommitmentID });
            ModelState.AddModelError("", submit.ErrorMessage);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteDocumentAsync()
        {           
            _ = await _mediator.Send(new CommandDocumentDelete() {  StudentDocumentId = DocumentId, StudentId = User.GetUserId() });
            return RedirectToPage("/Student/CommitmentDocuments", new { cid = CommitId });
        }

        public class Query : IRequest<CommitmentDocumentViewModel>
        {
            public int StudId { get; set; }
            public int CommitId { get; set; }

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
                    .Select(b => new {Status = b.CommitmentStatus.Code, ApprovalFlow = b.Agency.CommitmentApprovalWorkflow.Code}).FirstOrDefaultAsync();

                model.ApprovalFlow = lookupData.ApprovalFlow;
                model.Status = lookupData.Status;
                if (lookupData.Status == CommitmentProcessConst.RequestFinalDocs)
                    model.ApprovalFlow = CommitmentProcessConst.CommitmentApprovalFinal;
                if (request.CommitId > 0)
                {                    
                    var docs = await _efDB.CommitmentStudentDocument
                        .Include(m => m.StudentDocument).ThenInclude(m => m.DocumentType)
                        .Where(m => m.StudentCommitmentID == request.CommitId && m.StudentDocument.IsDeleted != true)
                        .WhereIf(lookupData.Status == CommitmentProcessConst.RequestFinalDocs, m => m.StudentDocument.DocumentType.Code != "TentativeOffer")
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
                        if(doc.DocumentTypeCode == "TentativeOffer")
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
               
                if(model.ApprovalFlow == CommitmentProcessConst.CommitmentApprovalFinal && model.SavedDocuments.Count == 2)
                {
                    model.CanSubmit = true;
                }
                if(model.ApprovalFlow == CommitmentProcessConst.CommitmentApprovalTentative && model.SavedDocuments.Count == 1)
                {
                    model.CanSubmit = true;
                }
                return model;
            }
        }

        public class Command : IRequest<CommandHandler.UploadCommandResult>
        {
            public CommitmentDocumentViewModel Model { get; set; }
            public int StudentId { get; set; }
            public string DocumentType { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandHandler.UploadCommandResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IAntiVirusHelper _avService;
            private readonly IConfiguration _appSettings;
            private readonly IAzureBlobService _blobService;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IAntiVirusHelper avService, IConfiguration appSettings, IAzureBlobService blobService, ILogger<CommandHandler> logger)
            {
                _efDB = efDB;
                _cache = cache;
                _avService = avService;
                _appSettings = appSettings;
                _blobService = blobService;
                _logger = logger;
            }

            public async Task<UploadCommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                IFormFile docToUpload = null;
                if (request.DocumentType == CommitmentProcessConst.CommitmentDocumentPositionDescription)
                    docToUpload = request.Model.PostionDescription;
                if (request.DocumentType == CommitmentProcessConst.CommitmentDocumentTentative)
                    docToUpload = request.Model.TenativeJobOffer;
                if (request.DocumentType == CommitmentProcessConst.CommitmentDocumentFinal)
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
                            var isFileAVClean = _avService.IsDocAVClean(fileStream, AVServer, request.StudentId.ToString());
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
                        fullPath = await _blobService.UploadDocumentStreamAsync(docToUpload, request.StudentId, "ST");
                        _logger.LogInformation($"Document upload to blob storage - {fullPath}");
                    }                   
                    
                    using var transaction = _efDB.Database.BeginTransaction();
                    var student = _efDB.Students.Where(m => m.StudentId == request.StudentId).FirstOrDefault();
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
                        .Where(m => m.StudentId == request.StudentId && m.StudentCommitmentId == request.Model.CommitmentID).FirstOrDefault();
                    commitmentRecord.CommitmentStudentDocuments.Add(new CommitmentStudentDocument()
                    {
                        StudentDocumentID = docToAdd.StudentDocumentId,
                        DateAdded = DateTime.UtcNow
                    });
                    await _efDB.SaveChangesAsync();
                    await transaction.CommitAsync();

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
            private readonly ICommitmentNotificationService _emailer;
            private readonly ICacheHelper _cache;
            private readonly ICommitmentProcessService _commitHelper;
            private readonly IStudentDashboardService _service;

            public SubmitCommandHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICommitmentNotificationService emailer, ICommitmentProcessService commitHelper, IStudentDashboardService service)
            {
                _efDB = efDB;
                _cache = cache;
                _emailer = emailer;
                _commitHelper = commitHelper;
                _service = service;
            }

            public async Task<bool> Handle(SubmitCommand request, CancellationToken cancellationToken)
            {
             
               var commitmentStatusList = await _cache.GetCommitmentStatusAsync();
               var theCommitment =  await _efDB.StudentCommitments
                    .Where(m => m.StudentCommitmentId == request.Model.CommitmentID)
                    .Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.Agency).ThenInclude(m => m.AgencyType)
                    .Include(m => m.Agency).ThenInclude(m => m.CommitmentApprovalWorkflow)
                    .Include(m => m.CommitmentType)   
                    .Include(m => m.CommitmentStatus)                    
                    .FirstOrDefaultAsync();

                //Set status 
                var nextStatusData = await _commitHelper.GetNextStatusForApprovalAsync(theCommitment.CommitmentStatus.Code, theCommitment.Agency.CommitmentApprovalWorkflow.Code);
                theCommitment.CommitmentStatusId = nextStatusData.StatusID;
                if(!theCommitment.DateSubmitted.HasValue || theCommitment.DateSubmitted.Value == DateTime.MinValue)
                {
                    //don't overwrite the original submit date
                    theCommitment.DateSubmitted = DateTime.UtcNow;
                }
                else
                {
                    theCommitment.LastModified = DateTime.UtcNow;
                }

                //send emails
                CommitmentNotificationRequest emailData = new() {
                    Commitment = theCommitment,
                    InstitutionID = theCommitment.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value
                };

               
                await _emailer.SendEmailWhenStudentSubmittedAsync(emailData);
                await _efDB.SaveChangesAsync();
                //update the commitment reporting fields on the funding table
                await _service.UpdateCommitmentReportedForDashboard(theCommitment.StudentId);
                return true;
            }
        }

    }

  

    
}
