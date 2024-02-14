using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System;
using static OPM.SFS.Web.Models.CommitmentModelViewModel;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.Models;
using Microsoft.AspNetCore.Http;
using OPM.SFS.Core.Shared;
using Microsoft.Extensions.Logging;
using static OPM.SFS.Web.SharedCode.CommitmentProcessService;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OPM.SFS.Web.Handlers;
using FluentValidation;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    public class EmploymentVerificationModel : PageModel
	{
		[BindProperty]
		public EVFCommitmentViewModel Data { get; set; }

        [FromQuery(Name = "cid")]
        public int CommitId { get; set; } = 0;

        [FromQuery(Name = "fid")]
        public int DocumentId { get; set; } = 0;

        private readonly IMediator _mediator;
		public EmploymentVerificationModel(IMediator mediator) => _mediator = mediator;

		public async Task OnGetAsync()
		{
			Data = await _mediator.Send(new EVFCommitmentQuery() { CommitId = CommitId });
		}

        public async Task<FileResult> OnGetViewDocument()
        {
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = DocumentId });
            return file;
        }
        public async Task<IActionResult> OnGetDeleteDocumentAsync()
        {
            _ = await _mediator.Send(new CommandDocumentDelete() { StudentDocumentId = DocumentId, StudentId = User.GetUserId() });
            return Redirect($"/Student/EmploymentVerification?cid={CommitId}");
        }

        public async Task<IActionResult> OnGetCancelEvfAsync()
        {           
            _ = await _mediator.Send(new CommandCancelEVF() { CommitmentId = CommitId, StudentId = User.GetUserId() });            
            return Redirect($"/Student/CommitmentVerification");
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                Data = await _mediator.Send(new EVFCommitmentQuery() { CommitId = Data.CommitmentId });
                return Page();
            }
            var result = await _mediator.Send(new CommandUpload()
            {
                Model = Data,
                StudentID = User.GetUserId(),
                CommitId = Data.CommitmentId
            });
            return Redirect($"/Student/EmploymentVerification?cid={Data.CommitmentId}");
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {

            var result = await _mediator.Send(new CommandEVFSubmit()
            {
                Model = Data,
                StudentID = User.GetUserId()
            });
            return Redirect($"/Student/CommitmentVerification");
        }
    }

    public class DocumentUploadValidator : AbstractValidator<EVFCommitmentViewModel>
    {
        public DocumentUploadValidator()
        {
            //RuleFor(x => x.Name).Matches("^[^><&]+$");
            RuleFor(x => x.UploadedDocument).NotNull().WithMessage("Document is required.");
            When(x => x.Documents != null, () =>
            {
                RuleFor(x => x.UploadedDocument.Length).NotNull().LessThanOrEqualTo(5242880)
                    .WithMessage("File size exceeds 5MB");
                RuleFor(x => x.UploadedDocument.ContentType).Must((o, positiondescription) => { return IsValidDocType(o.UploadedDocument.ContentType); })
                    .WithMessage("Files must be one of the following formats: GIF, JPG, JPEG, PNG, RTF, TXT, PDF or Word (DOC or DOCX)"); 
            });
        }

        private bool IsValidDocType(string contentType)
        {
            if (contentType.ToLower() == "application/pdf") //pdf
                return true;
            if (contentType.ToLower() == "application/msword") //word (doc)
                return true;
            if (contentType.ToLower() == "application/vnd.openxmlformats-officedocument.wordprocessingml.document") //word (docx)
                return true;
            if (contentType.ToLower() == "text/plain") //txt
                return true;
            if (contentType.ToLower() == "image/gif") //gif
                return true;
            if (contentType.ToLower() == "image/jpeg") //jpg,jpeg
                return true;
            if (contentType.ToLower() == "image/png") //png
                return true;
            if (contentType.ToLower() == "application/rtf") //rtf
                return true;
            return false;
        }
    }

    public class EVFCommitmentQuery : IRequest<EVFCommitmentViewModel>
	{
		public int CommitId { get; set; }
	}

	public class GetEVFCommitmentsHandler : IRequestHandler<EVFCommitmentQuery, EVFCommitmentViewModel>
	{
		private readonly ScholarshipForServiceContext _efDB;
		private readonly ICacheHelper _cache;
		public GetEVFCommitmentsHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
		{
			_efDB = efDB;
			_cache = cache;
		}

		public async Task<EVFCommitmentViewModel> Handle(EVFCommitmentQuery request, CancellationToken cancellationToken)
		{
            var agencies = await _cache.GetAgenciesAsync();
            var _theCommitment = await _efDB.StudentCommitments
                        .Where(m => m.StudentCommitmentId == request.CommitId)
                        .Select(p => new EVFCommitmentViewModel()
                        {
                            CommitmentId = p.StudentCommitmentId,
                            StudentId = p.StudentId,
                            Agency = p.Agency.Name,
                            SubAgency = p.Agency.ParentAgencyId > 0 ? p.Agency.Name : "",
                            ParentAgencyId = p.Agency.ParentAgencyId,
                            JobTitle = p.JobTitle,
                            StartDate = p.StartDate.Value.ToShortDateString(),
                            StatusDisplay = p.CommitmentStatus.StudentDisplay,
                            StatusCode = p.CommitmentStatus.Value,
                            StatusDescription = p.CommitmentStatus.Description,
                            Training = p.EmploymentVerification.Training,
                            TakingRemedialTraining = !p.EmploymentVerification.TakingRemedialTraining.HasValue ? null :
                             (p.EmploymentVerification.TakingRemedialTraining.Value ? "Yes" : "No"),
                            IsSamePosition = !p.EmploymentVerification.IsSamePosition.HasValue ? null :
                             (p.EmploymentVerification.IsSamePosition.Value ? "Yes" : "No"),
                        }).FirstOrDefaultAsync();

            var evfDocuments = await _efDB.StudentDocuments.Where(m => m.DocumentType.Code == "EVF" && m.StudentId == _theCommitment.StudentId)
                                .Where(m => !m.IsDeleted.Value)
                                .Select(m => new
                                {
                                    DocumentType = m.DocumentType.Name,
                                    DocumentName = m.FileName,
                                    DocumentId = m.StudentDocumentId
                                }).ToListAsync();

			var temp = _theCommitment.Agency;
            _theCommitment.Agency = _theCommitment.ParentAgencyId != null && _theCommitment.ParentAgencyId > 0 ? agencies.Where(m => m.AgencyId == _theCommitment.ParentAgencyId).FirstOrDefault().Name : _theCommitment.Agency;
            _theCommitment.SubAgency = _theCommitment.ParentAgencyId != null ? temp : "N/A";
            _theCommitment.AgencySubAgencyDisplay = _theCommitment.SubAgency == "N/A" ? _theCommitment.Agency : $"{_theCommitment.Agency}, {_theCommitment.SubAgency}";
            _theCommitment.AnswerOptions =  new[] { "Yes", "No" };
            _theCommitment.Documents = new List<EvfDocumentInfo>();
            foreach(var document in evfDocuments)
            {
                _theCommitment.Documents.Add(new EvfDocumentInfo() { EVFDocumentName = document.DocumentName, EVFDocumentId= document.DocumentId, CommitmentId = request.CommitId });
            }           
            return _theCommitment;
		}
	}

    public class CommandUpload : IRequest<CommandUploadResult>
    {
        public EVFCommitmentViewModel Model { get; set; }
        public int StudentID { get; set; }
        public int CommitId { get; set; }
    }

    public class CommandUploadHandler : IRequestHandler<CommandUpload, CommandUploadResult>
    {
        private readonly ScholarshipForServiceContext _efDB;
        private readonly IAzureBlobService _blobService;
        private readonly ILogger<CommandHandler> _logger;

        public CommandUploadHandler(ScholarshipForServiceContext efDB, IAzureBlobService blobService, ILogger<CommandHandler> logger)
        {
            _efDB= efDB;
            _blobService= blobService;
            _logger= logger;
        }
        public async Task<CommandUploadResult> Handle(CommandUpload request, CancellationToken cancellationToken)
        {
            //upload to storage
            try
            {
                
                var fullPath = await _blobService.UploadDocumentStreamAsync(request.Model.UploadedDocument, request.StudentID, "ST");
                var docTypeiD = await _efDB.DocumentTypes.Where(m => m.Code == "EVF").Select(m => m.DocumentTypeId).FirstOrDefaultAsync();
                _logger.LogInformation($"Document upload to blob storage - {fullPath}");

                //add document link to database 
                StudentDocument newDocument = new StudentDocument()
                {
                    StudentId = request.StudentID,
                    DateCreated = DateTime.UtcNow,
                    FileName = request.Model.UploadedDocument.FileName,
                    FilePath = fullPath,
                    IsDeleted = false,
                    DocumentTypeId = docTypeiD,
                    UserId = request.StudentID.ToString()

                };


                await _efDB.StudentDocuments.AddAsync(newDocument);
                await _efDB.SaveChangesAsync();
                var existingEVF = await _efDB.EmploymentVerification.Where(m => m.StudentCommitmentId == request.Model.CommitmentId).FirstOrDefaultAsync();
                if (existingEVF != null)
                {
                    existingEVF.EVFDocumentId = newDocument.StudentDocumentId;
                    await _efDB.SaveChangesAsync();
                }
                else
                {
                    EmploymentVerification newEVF = new();
                    newEVF.StudentCommitmentId = request.Model.CommitmentId;
                    newEVF.EVFDocumentId = newDocument.StudentDocumentId;
                    await _efDB.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new CommandUploadResult() { ErrorMessage = "Unexpected error has occurred.", IsSuccess = false };
            }
            return new CommandUploadResult() { ErrorMessage = "", IsSuccess= true };
        }
    }

    public class CommandUploadResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CommandEVFSubmit : IRequest<bool>
    {
        public EVFCommitmentViewModel Model { get; set; }
        public int StudentID { get; set; }
    }

    public class CommandEVFSubmitHandler : IRequestHandler<CommandEVFSubmit, bool>
    {
        private readonly ScholarshipForServiceContext _efDB;
        public CommandEVFSubmitHandler(ScholarshipForServiceContext efDB)
        {
            _efDB = efDB;
        }

        public async Task<bool> Handle(CommandEVFSubmit request, CancellationToken cancellationToken)
        {
            //get EVF by commitmentID
            var existingEVF = await _efDB.EmploymentVerification.Where(m => m.StudentCommitmentId == request.Model.CommitmentId).FirstOrDefaultAsync();
            if (existingEVF != null)
            {
                existingEVF.TakingRemedialTraining = request.Model.TakingRemedialTraining == "Yes";
                existingEVF.Training = request.Model.Training;
                existingEVF.IsSamePosition = request.Model.IsSamePosition == "Yes";
                existingEVF.PositionEndDate = request.Model.PositionEndDate;
                existingEVF.HasNewCommitment = request.Model.HasNewCommitment == "Yes";
                existingEVF.Status = "Pending Review";
                await _efDB.SaveChangesAsync();
            }
            else
            {
                //Add new EVF
                EmploymentVerification newEVF = new();
                newEVF.StudentCommitmentId = request.Model.CommitmentId;
                newEVF.StudentId = request.StudentID;
                newEVF.TakingRemedialTraining = request.Model.TakingRemedialTraining == "Yes";
                newEVF.Training = request.Model.Training;
				newEVF.IsSamePosition = request.Model.IsSamePosition == "Yes";
				newEVF.PositionEndDate = request.Model.PositionEndDate;
				newEVF.HasNewCommitment = request.Model.HasNewCommitment == "Yes";
                newEVF.Status = "Pending Review";
                _efDB.EmploymentVerification.Add(newEVF);
                await _efDB.SaveChangesAsync();
            }
            return true;
        }
    }

    public class CommandCancelEVF : IRequest<bool>
    {
        public int CommitmentId { get; set; }
        public int StudentId { get; set; }
    }

    public class CommandCancelEVFHandler : IRequestHandler<CommandCancelEVF, bool>
    {
        private readonly ScholarshipForServiceContext _efDB;
        private readonly IAzureBlobService _blobService;
        public CommandCancelEVFHandler(ScholarshipForServiceContext efDB, IAzureBlobService blobService)
        {
            _efDB = efDB;
            _blobService = blobService;
        }
        public async Task<bool> Handle(CommandCancelEVF request, CancellationToken cancellationToken)
        {
            //get all EVF documents 
            var efDocuments = await _efDB.StudentDocuments
                .Include(m => m.DocumentType)
                .Where(m => m.DocumentType.Code == "EVF")
                .Where(m => m.StudentId == request.StudentId)
                .Where(m => !m.IsDeleted.Value)
                .ToListAsync();

            //foreach (var doc in efDocuments)
            //{
            //    await _blobService.DeleteDocumentAsync(doc.FilePath);
            //}
            if (efDocuments != null && efDocuments.Any() )
            {
                _efDB.StudentDocuments.RemoveRange(efDocuments);
                await _efDB.SaveChangesAsync();
            }
           
            //Delete  EVF data tied to the commitmentid
            var evfData = await _efDB.EmploymentVerification.Where(m => m.StudentCommitmentId == request.CommitmentId).FirstOrDefaultAsync();
            if(evfData != null)
            {
                _efDB.EmploymentVerification.Remove(evfData);
                await _efDB.SaveChangesAsync();
            }
            return true;
        }
    }
}
