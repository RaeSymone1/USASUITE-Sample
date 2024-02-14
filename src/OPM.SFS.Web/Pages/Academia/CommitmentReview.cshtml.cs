using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models.Academia;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]
    public class CommitmentReviewModel : PageModel
    {

        [BindProperty]
        public CommitmentReviewViewModel Data { get; set; }

        [FromQuery(Name = "cid")]
        public int CommitId { get; set; } = 0;

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;
        [FromQuery(Name = "fid")]
        public int DocumentId { get; set; } = 0;
        private readonly IMediator _mediator;

        public CommitmentReviewModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { CommitId = CommitId, StudentID = StudentID });
        }

        public async Task<FileResult> OnGetViewDocument(int id)
        {
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = DocumentId });
            return file;
        }

        public async Task<IActionResult> OnPostApproveAsync()
        {
            var submit = await _mediator.Send(new CommandUpdateStatus() { CommitmentID = Data.CommitmentId, Action = "PIApprovalPendingFOL", PiID = User.GetUserId() });
            return Redirect($"/Academia/Commitments?cid={Data.CommitmentId}");
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            var submit = await _mediator.Send(new CommandUpdateStatus() { CommitmentID = Data.CommitmentId, Action = "PIReject", PiID = User.GetUserId() });
            return Redirect($"/Academia/Commitments?cid={Data.CommitmentId}");
        }

        public class Query : IRequest<CommitmentReviewViewModel>
        {
            public int CommitId { get; set; }
            public int StudentID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, CommitmentReviewViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly ICryptoHelper _crypto;
            public readonly ICommitmentProcessService _commitHelper;


            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICryptoHelper crypto, ICommitmentProcessService commitHelper)
            {
                _efDB = efDB;
                _cache = cache;
                _crypto = crypto;
                _commitHelper = commitHelper;
            }

            public async Task<CommitmentReviewViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                CommitmentReviewViewModel model = new CommitmentReviewViewModel();
                model.CommitmentId = request.CommitId;
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                //populate the select lists
                model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                model.CommitmentTypeList = new SelectList(await _cache.GetCommitmentTypeAsync(), nameof(CommitmentType.CommitmentTypeId), nameof(CommitmentType.Name));
                model.PayRateList = new SelectList(await _cache.GetPayRateListAsync(), nameof(SalaryType.SalaryTypeId), nameof(SalaryType.Name));
                model.JobSearchTypeList = new SelectList(await _cache.GetJobSearchTypeAsync(), nameof(JobSearchType.JobSearchTypeID), nameof(JobSearchType.Name));
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));

                var agencies = await _cache.GetAgenciesAsync();


                var commitmentData = _efDB.StudentCommitments.Where(m => m.StudentCommitmentId == request.CommitId)
                    .Include(m => m.Agency).ThenInclude(m => m.CommitmentApprovalWorkflow)
                    .Include(m => m.MentorContact)
                    .Include(m => m.SupervisorContact)
                    .Include(m => m.Address)
                    .Include(m => m.CommitmentStatus)
                    .Include(m => m.Student)
                    .Include(m => m.CommitmentType)
                    .AsNoTracking()
                    .FirstOrDefault();

                model.FirstName = commitmentData.Student.FirstName;
                model.LastName = commitmentData.Student.LastName;
                var ssn = _crypto.Decrypt(commitmentData.Student.Ssn, GlobalConfigSettings);
                model.FormattedSSN = $"xxx-xx-{ssn.Substring(ssn.Length - 4)}";
                model.AgencyType = commitmentData.Agency.AgencyTypeId;
                model.Status = commitmentData.CommitmentStatus.Code;
                model.AgencyApprovalWorkflow = commitmentData.Agency.CommitmentApprovalWorkflow.Code.ToLower();
                if (commitmentData.Agency.ParentAgencyId.HasValue && commitmentData.Agency.ParentAgencyId.Value > 0)
                {
                    //populate the agency lists

                    model.AgencyList = new SelectList(agencies.OrderBy(m => m.Name).Where(m => m.AgencyTypeId == commitmentData.Agency.AgencyTypeId).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));

                    model.SubAgencyList = new SelectList(agencies.Where(m => m.ParentAgencyId == commitmentData.Agency.ParentAgencyId.Value)
                                                    .OrderBy(m => m.Name).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));


                    model.Agency = agencies.Where(m => m.AgencyId == commitmentData.Agency.ParentAgencyId.Value).Select(m => m.AgencyId).FirstOrDefault();
                    model.SubAgency = commitmentData.Agency.AgencyId;
                }
                else
                {
                    model.AgencyList = new SelectList(agencies.OrderBy(m => m.Name).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));
                    model.Agency = commitmentData.Agency.AgencyId;
                }
                model.AgencyApprovalWorkflow = commitmentData.Agency.CommitmentApprovalWorkflow.Code;
                //model.ShowForm = commitmentData.Agency.CommitmentApprovalWorkflow.Code;
				string startDate = commitmentData.StartDate.HasValue ? commitmentData.StartDate.Value.ToShortDateString() : "";
                model.ShowForm = _commitHelper.GetFormByStatus(commitmentData.CommitmentStatus.Code, commitmentData.Agency.CommitmentApprovalWorkflow.Code, startDate);

                model.Justification = commitmentData.Justification;
                model.JobTitle = commitmentData.JobTitle;
                model.Agency = commitmentData.AgencyId.Value;
                model.CommitmentType = commitmentData.CommitmentTypeId.Value;
                model.PayRate = commitmentData.SalaryTypeId;
                model.SalaryMin = commitmentData.SalaryMinimum.HasValue ? decimal.Round(commitmentData.SalaryMinimum.Value, 2, MidpointRounding.AwayFromZero) : commitmentData.SalaryMinimum;
                model.SalaryMax = commitmentData.SalaryMaximum.HasValue ? decimal.Round(commitmentData.SalaryMaximum.Value, 2, MidpointRounding.AwayFromZero) : commitmentData.SalaryMaximum;
                model.PayPlan = commitmentData.PayPlan;
                model.Series = commitmentData.Series;
                model.Grade = commitmentData.Grade;
                if (commitmentData.Address is not null)
                {
                    model.Address1 = commitmentData.Address.LineOne;
                    model.Address2 = commitmentData.Address.LineTwo;
                    model.Address3 = commitmentData.Address.LineThree;
                    model.City = commitmentData.Address.City;
                    model.State = commitmentData.Address.StateId;
                    model.PostalCode = commitmentData.Address.PostalCode;
                }
                if (commitmentData.SupervisorContact is not null)
                {
                    model.SupervisorFirstName = commitmentData.SupervisorContact.FirstName;
                    model.SupervisorLastName = commitmentData.SupervisorContact.LastName;
                    model.SupervisorEmailAddress = commitmentData.SupervisorContact.Email;
                    if (!string.IsNullOrWhiteSpace(commitmentData.SupervisorContact.Phone))
                    {
                        var phoneChars = commitmentData.SupervisorContact.Phone.ToCharArray().ToList();
                        model.SupervisorPhoneAreaCode = string.Join("", phoneChars.Take(3));
                        model.SupervisorPhonePrefix = string.Join("", phoneChars.Skip(3).Take(3));
                        model.SupervisorPhoneSuffix = string.Join("", phoneChars.Skip(6).Take(4));
                    }
                    model.SupervisorPhoneExtension = commitmentData.SupervisorContact.PhoneExt;
                }
                if (commitmentData.MentorContact is not null)
                {
                    model.MentorFirstName = commitmentData.MentorContact.FirstName;
                    model.MentorLastName = commitmentData.MentorContact.LastName;
                    model.MentorEmailAddress = commitmentData.MentorContact.Email;
                    if (!string.IsNullOrWhiteSpace(commitmentData.MentorContact.Phone))
                    {
                        var phoneChars = commitmentData.MentorContact.Phone.ToCharArray().ToList();
                        model.MentorPhoneAreaCode = string.Join("", phoneChars.Take(3));
                        model.MentorPhonePrefix = string.Join("", phoneChars.Skip(3).Take(3));
                        model.MentorPhoneSuffix = string.Join("", phoneChars.Skip(6).Take(4));
                    }

                    model.MentorPhoneExtension = commitmentData.MentorContact.PhoneExt;
                }
                if (commitmentData.StartDate.HasValue)
                {
                    model.StartDateDay = commitmentData.StartDate.Value.Day.ToString();
                    model.StartDateMonth = commitmentData.StartDate.Value.Month.ToString();
                    model.StartDateYear = commitmentData.StartDate.Value.Year.ToString();
                }
                if (commitmentData.EndDate.HasValue)
                {
                    model.EndDateDay = commitmentData.EndDate.HasValue ? commitmentData.EndDate.Value.Day.ToString() : "";
                    model.EndDateMonth = commitmentData.EndDate.HasValue ? commitmentData.EndDate.Value.Month.ToString() : "";
                    model.EndDateYear = commitmentData.EndDate.HasValue ? commitmentData.EndDate.Value.Year.ToString() : "";
                }

                model.JobSearchType = commitmentData.JobSearchTypeId;


                var docs = await _efDB.CommitmentStudentDocument
                        .Where(m => m.StudentCommitmentID == request.CommitId && m.StudentDocument.IsDeleted != true)
                        .Select(m => new
                        {
                            ID = m.StudentDocumentID,
                            Type = m.StudentDocument.DocumentType.Name,
                            Name = m.StudentDocument.FileName
                        })
                        .ToListAsync();

                model.SavedDocuments = new List<CommitmentReviewViewModel.SavedDocument>();
                foreach (var doc in docs)
                {
                    model.SavedDocuments.Add(new CommitmentReviewViewModel.SavedDocument()
                    {
                        Id = doc.ID,
                        Name = doc.Name.Contains('_') ? doc.Name.Substring(doc.Name.IndexOf("_") + 1) : doc.Name,
                        Type = doc.Type
                    });
                }
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
            private readonly IConfiguration _appSettings;
            private readonly IAzureBlobService _blobService;

            public QueryDocumentViewHandler(IDocumentRepository document, IConfiguration appSettings, IAzureBlobService blobService)
            {
                _documentRepo = document;
                _appSettings = appSettings;
                _blobService = blobService;
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
        public class CommandUpdateStatus : IRequest<bool>
        {
            public int CommitmentID { get; set; }
            public string Action { get; set; }
            public int PiID { get; set; }
            public int InstitutionID { get; set; }
        }

        public class CommandUpdateStatusHandler : IRequestHandler<CommandUpdateStatus, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly ICommitmentNotificationService _emailer;

            public CommandUpdateStatusHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICommitmentNotificationService emailer)
            {
                _efDB = efDB;
                _cache = cache;
                _emailer = emailer;
            }


            public async Task<bool> Handle(CommandUpdateStatus request, CancellationToken cancellationToken)
            {

                var theCommitment = await _efDB.StudentCommitments
                    .Where(m => m.StudentCommitmentId == request.CommitmentID)
                    .Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.Agency).ThenInclude(m => m.AgencyType)
                    .Include(m => m.CommitmentType)
                    .FirstOrDefaultAsync();
                var statusOptions = await _cache.GetCommitmentStatusAsync();

                //Since PIs can only recommend approval or rejects need to always set the status to PO Review and log the PIRcommendation in the 
                //Student table. 
                var approveStatusID = statusOptions.FirstOrDefault(m => m.Code == CommitmentProcessConst.ApprovalPendingPO).CommitmentStatusID;
                theCommitment.CommitmentStatusId = approveStatusID;
                theCommitment.LastModified = DateTime.UtcNow;
                theCommitment.PIRecommendation = request.Action.Contains("Approval") ? "Approve" : "Reject";
                theCommitment.LastUpdatedByPIID = request.PiID;
                await _efDB.SaveChangesAsync();

                //send emails
                CommitmentNotificationRequest emailData = new()
                {
                    Commitment = theCommitment,
                    InstitutionID = theCommitment.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value
                };

                await _emailer.SendEmaislWhenPIReviewsAsync(emailData);

                return true;
            }
        }
    }
}
