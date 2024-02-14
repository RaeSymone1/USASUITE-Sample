using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public interface ICommitmentProcessService
    {
        Task<CommitmentProcessService.Result> StudentAddNewCommitmentAsync(CommitmentModelViewModel model);
        Task<CommitmentProcessService.Result> StudentUpdateCommitmentAsync(CommitmentModelViewModel model);
        Task<bool> IsDuplicateCommitmentAsync(CommitmentModelViewModel model);
        Task<CommitmentProcessService.NextStatusData> GetNextStatusForApprovalAsync(string currentStatus, string agencyApprovalFlow, bool isAdminApproval = false);
        Task<CommitmentProcessService.NextStatusData> GetNextStatusForRejectAsync();
        DateTime CalculateEndDate(double ServiceOwed, DateTime startDate);
        string GetFormByStatus(string status, string agencyApprovalFlow, string startdate);

	}

    public class CommitmentProcessService : ICommitmentProcessService
    {
        private readonly ScholarshipForServiceContext _efDB;
        private readonly ICacheHelper _cache;

        public CommitmentProcessService(ScholarshipForServiceContext efDB, ICacheHelper cache)
        {
            _efDB = efDB;
            _cache = cache;
        }

        public async Task<Result> StudentAddNewCommitmentAsync(CommitmentModelViewModel model)
        {
            var isDupliclicate = await IsDuplicateCommitmentAsync(model);
            if (isDupliclicate) return new Result() { IsSuccess = false, Message = "This commitment matches an already submitted commitment. Please contact the Program Office at sfs@opm.gov" };

            var studentRecord = _efDB.Students
                  .Where(m => m.StudentId == Convert.ToInt32(model.StudentID))
                  .Include(m => m.StudentCommitments)
                  .Include(m => m.StudentInstitutionFundings)
                  .FirstOrDefault();
            StudentCommitment newCommitment;
            var statusOptions = await _cache.GetCommitmentStatusAsync();

            if (model.ShowForm.Equals(CommitmentProcessConst.CommitmentApprovalTentative, StringComparison.CurrentCultureIgnoreCase))
            {
                newCommitment = new StudentCommitment()
                {
                    AgencyId = model.SubAgency > 0 ? model.SubAgency : model.Agency,
                    CommitmentTypeId = model.CommitmentType,
                    JobTitle = model.JobTitle,
                    Justification = model.Justification,
                    DateInserted = DateTime.UtcNow,
                    CommitmentStatusId = statusOptions.Where(m => m.Code == CommitmentProcessConst.Incomplete).FirstOrDefault().CommitmentStatusID
                };
            }
            else
            {
                DateTime PostAvailGradDate = (DateTime)studentRecord.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate;
                DateTime ExpectedGradDate = (DateTime)studentRecord.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate;
                DateTime StartDate = Convert.ToDateTime($"{model.StartDateMonth}/{model.StartDateDay}/{model.StartDateYear}");
                var commitmentTypes = await _cache.GetCommitmentTypeAsync();
                string commitmentTypeName = commitmentTypes.Where(m => m.CommitmentTypeId == model.CommitmentType).Select(m => m.Name).First();
                if (commitmentTypeName.Contains("Internship") && (ExpectedGradDate.CompareTo(StartDate) <= 0))
                {
                    return new Result() { IsSuccess = false, Message = "The projected start date is invalid" };
                }
                if (commitmentTypeName.Contains("Postgraduate") && (StartDate.CompareTo(PostAvailGradDate) < 0))
                {
                    return new Result() { IsSuccess = false, Message = "The projected start date is invalid" };
                }

                newCommitment = new StudentCommitment()
                {
                    AgencyId = model.SubAgency > 0 ? model.SubAgency : model.Agency,
                    CommitmentTypeId = model.CommitmentType,
                    JobTitle = model.JobTitle,
                    SalaryTypeId = model.PayRate,
                    SalaryMinimum = Convert.ToDecimal(model.SalaryMin),
                    SalaryMaximum = Convert.ToDecimal(model.SalaryMax),
                    PayPlan = !string.IsNullOrWhiteSpace(model.PayPlan) ? model.PayPlan.Trim() : "",
                    Series = !string.IsNullOrWhiteSpace(model.Series) ? model.Series.Trim() : "",
                    Grade = !string.IsNullOrWhiteSpace(model.Grade) ? model.Grade.Trim() : "",
                    DateInserted = DateTime.UtcNow,
                    CommitmentStatusId = _efDB.CommitmentStatus.Where(m => m.Code == CommitmentProcessConst.Incomplete).AsNoTracking().FirstOrDefault().CommitmentStatusID,
                    Address = new Address()
                    {
                        LineOne = "",
                        Country = model.Country,
                        City = model.City,
                        StateId = model.State.Value,
                        PostalCode = model.PostalCode
                    },
                    SupervisorContact = new Contact()
                    {
                        FirstName = model.SupervisorFirstName,
                        LastName = model.SupervisorLastName,
                        Email = model.SupervisorEmailAddress,
                        Phone = $"{model.SupervisorPhoneAreaCode}{model.SupervisorPhonePrefix}{model.SupervisorPhoneSuffix}",
                        PhoneExt = model.SupervisorPhoneExtension

                    },
                    MentorContact = new Contact()
                    {
                        FirstName = model.MentorFirstName,
                        LastName = model.MentorLastName,
                        Email = model.MentorEmailAddress,
                        Phone = $"{model.MentorPhoneAreaCode}{model.MentorPhonePrefix}{model.MentorPhoneSuffix}",
                        PhoneExt = model.MentorPhoneExtension
                    },
                    StartDate = Convert.ToDateTime($"{model.StartDateMonth}/{model.StartDateDay}/{model.StartDateYear}"),
                    JobSearchTypeId = model.JobSearchType
                };

                if (!string.IsNullOrWhiteSpace(model.EndDateMonth) && !string.IsNullOrWhiteSpace(model.EndDateDay) && !string.IsNullOrWhiteSpace(model.EndDateYear))
                    newCommitment.EndDate = Convert.ToDateTime($"{model.EndDateMonth}/{model.EndDateDay}/{model.EndDateYear}");
                newCommitment.DateInserted = DateTime.UtcNow;
            }
            studentRecord.StudentCommitments.Add(newCommitment);
            _ = await _efDB.SaveChangesAsync();
            return new Result() { IsSuccess = true, ID = newCommitment.StudentCommitmentId };
        }

        public async Task<Result> StudentUpdateCommitmentAsync(CommitmentModelViewModel model)
        {
            var isDupliclicate = await IsDuplicateCommitmentAsync(model);
            if (isDupliclicate) return new Result() { IsSuccess = false, Message = "This commitment matches an already submitted commitment. Please contact the Program Office at sfs@opm.gov" };

            var editCommitment = await _efDB.StudentCommitments
                .Include(i => i.Student).ThenInclude(i => i.StudentInstitutionFundings)
                  .Include(i => i.Address)
                  .Include(i => i.MentorContact)
                  .Include(i => i.SupervisorContact)
                  .Where(m => m.StudentId == model.StudentID && m.StudentCommitmentId == model.CommitmentId)
                  .FirstOrDefaultAsync();

            editCommitment.AgencyId = model.SubAgency > 0 ? model.SubAgency : model.Agency;
            editCommitment.CommitmentTypeId = model.CommitmentType;
            editCommitment.JobTitle = model.JobTitle;


            if (model.ShowForm == CommitmentProcessConst.CommitmentApprovalTentative)
            {
                editCommitment.Justification = model.Justification;
            }
            else
            {                
                DateTime ExpectedGradDate = (DateTime)editCommitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate;
                DateTime StartDate = Convert.ToDateTime($"{model.StartDateMonth}/{model.StartDateDay}/{model.StartDateYear}");
                var commitmentTypes = await _cache.GetCommitmentTypeAsync();
                string commitmentTypeName = commitmentTypes.Where(m => m.CommitmentTypeId == model.CommitmentType).Select(m => m.Name).First();
                if (commitmentTypeName.Contains("Internship") && StartDate > ExpectedGradDate)
                {
                    return new Result() { IsSuccess = false, Message = "The projected start date is invalid" };
                }

                if (commitmentTypeName.Contains("Postgraduate") && StartDate < ExpectedGradDate)
                {
                    return new Result() { IsSuccess = false, Message = "The projected start date is invalid" };
                }

                editCommitment.SalaryTypeId = model.PayRate;
                editCommitment.JobSearchTypeId = model.JobSearchType;
                editCommitment.StartDate = Convert.ToDateTime($"{model.StartDateMonth}/{model.StartDateDay}/{model.StartDateYear}");
                editCommitment.SalaryMinimum = model.SalaryMin;
                editCommitment.SalaryMaximum = model.SalaryMax;
                editCommitment.PayPlan = !string.IsNullOrWhiteSpace(model.PayPlan) ? model.PayPlan.Trim() : "";
                editCommitment.Series = !string.IsNullOrWhiteSpace(model.Series) ? model.Series.Trim() : ""; ;
                editCommitment.Grade = !string.IsNullOrWhiteSpace(model.Grade) ? model.Grade.Trim() : ""; ;

                //address FK
                if (editCommitment.Address is null)
                {
                    editCommitment.Address = new Address()
                    {
                        LineOne = "",
                        City = model.City,
                        StateId = model.State.Value,
                        PostalCode = model.PostalCode,
                        Country = "United States"
                    };
                }
                else
                {
                    editCommitment.Address.City = model.City;
                    editCommitment.Address.StateId = model.State.Value;
                    editCommitment.Address.PostalCode = model.PostalCode;
                    editCommitment.Address.Country = "United States";
                }

                //Supervisor Contact
                if (editCommitment.SupervisorContact is null)
                {
                    editCommitment.SupervisorContact = new Contact()
                    {
                        FirstName = model.SupervisorFirstName,
                        LastName = model.SupervisorLastName,
                        Email = model.SupervisorEmailAddress,
                        Phone = $"{model.SupervisorPhoneAreaCode}{model.SupervisorPhonePrefix}{model.SupervisorPhoneSuffix}",
                        PhoneExt = model.SupervisorPhoneExtension

                    };
                }
                else
                {
                    editCommitment.SupervisorContact.FirstName = model.SupervisorFirstName;
                    editCommitment.SupervisorContact.LastName = model.SupervisorLastName;
                    editCommitment.SupervisorContact.Email = model.SupervisorEmailAddress;
                    editCommitment.SupervisorContact.Phone = $"{model.SupervisorPhoneAreaCode}{model.SupervisorPhonePrefix}{model.SupervisorPhoneSuffix}";
                    editCommitment.SupervisorContact.PhoneExt = model.SupervisorPhoneExtension;

                }

                //mentor contact
                if (editCommitment.MentorContact is null)
                {
                    editCommitment.MentorContact = new Contact()
                    {
                        FirstName = model.MentorFirstName,
                        LastName = model.MentorLastName,
                        Email = model.MentorEmailAddress,
                        Phone = $"{model.MentorPhoneAreaCode}{model.MentorPhonePrefix}{model.MentorPhoneSuffix}",
                        PhoneExt = model.MentorPhoneExtension
                    };
                }
                else
                {
                    editCommitment.MentorContact.FirstName = model.MentorFirstName;
                    editCommitment.MentorContact.LastName = model.MentorLastName;
                    editCommitment.MentorContact.Email = model.MentorEmailAddress;
                    editCommitment.MentorContact.Phone = $"{model.MentorPhoneAreaCode}{model.MentorPhonePrefix}{model.MentorPhoneSuffix}";
                    editCommitment.MentorContact.PhoneExt = model.MentorPhoneExtension;

                }
                if (!string.IsNullOrWhiteSpace(model.EndDateMonth) && !string.IsNullOrWhiteSpace(model.EndDateDay) && !string.IsNullOrWhiteSpace(model.EndDateYear))
                    editCommitment.EndDate = Convert.ToDateTime($"{model.EndDateMonth}/{model.EndDateDay}/{model.EndDateYear}");
            }

            await _efDB.SaveChangesAsync();
            return new Result() { IsSuccess = true, ID = editCommitment.StudentCommitmentId };
        }

        public string GetFormByStatus(string status, string agencyApprovalFlow, string startdate)
        {
			if (agencyApprovalFlow == CommitmentProcessConst.CommitmentApprovalTentative)
            {
                if(status == CommitmentProcessConst.Approved && !string.IsNullOrWhiteSpace(startdate))
                {
					return CommitmentProcessConst.CommitmentApprovalFinal;
				}
                if (status == CommitmentProcessConst.RequestFinalDocs || status == CommitmentProcessConst.FinalDocsPendingApproval)
                {
                    return CommitmentProcessConst.CommitmentApprovalFinal;
				}
            }
            return agencyApprovalFlow;

		}

        public async Task<NextStatusData> GetNextStatusForApprovalAsync(string currentStatus, string agencyApprovalFlow, bool isAdminApproval = false)
        {
            var commitmentStatusList = await _cache.GetCommitmentStatusAsync();
            if (agencyApprovalFlow == CommitmentProcessConst.CommitmentApprovalTentative)
            {
                if (currentStatus == CommitmentProcessConst.Incomplete)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.ApprovalPendingPI).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };
                }

                if (currentStatus == CommitmentProcessConst.ApprovalPendingPI && isAdminApproval)
                {
                    //If admin is approving on behalf of the PI then skip the PO Approval status...
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.RequestFinalDocs).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };
                }

                if (currentStatus == CommitmentProcessConst.ApprovalPendingPI)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.ApprovalPendingPO).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };
                }

                if (currentStatus == CommitmentProcessConst.ApprovalPendingPO)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.RequestFinalDocs).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };
                }

                if (currentStatus == CommitmentProcessConst.RequestFinalDocs)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.FinalDocsPendingApproval).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };
                }

                if (currentStatus == CommitmentProcessConst.FinalDocsPendingApproval)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.Approved).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };
                }

            }
            else
            {
                if (currentStatus == CommitmentProcessConst.Incomplete)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.ApprovalPendingPO).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };

                }
                if (currentStatus == CommitmentProcessConst.ApprovalPendingPO)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.Approved).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };

                }

                if (currentStatus == CommitmentProcessConst.FinalDocsPendingApproval)
                {
                    var statusData = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.Approved).FirstOrDefault();
                    return new NextStatusData() { StatusID = statusData.CommitmentStatusID, StatusCode = statusData.Code, StatusName = statusData.StudentDisplay };

                }

            }
            return null;
        }

        public async Task<NextStatusData> GetNextStatusForRejectAsync()
        {
            var commitmentStatusList = await _cache.GetCommitmentStatusAsync();
            var rejectedCode = commitmentStatusList.Where(m => m.Code == CommitmentProcessConst.Rejected).FirstOrDefault();
            return new NextStatusData() { StatusID = rejectedCode.CommitmentStatusID, StatusCode = rejectedCode.Code, StatusName = rejectedCode.Value };

        }

        public async Task<bool> IsDuplicateCommitmentAsync(CommitmentModelViewModel model)
        {
            var existingCommitmentID = await _efDB.StudentCommitments
                  .Where(m => m.StudentId == model.StudentID && m.IsDeleted != true)
                  .Where(m => m.AgencyId == model.Agency && m.CommitmentTypeId == model.CommitmentType)
                 .Select(m => m.StudentCommitmentId).FirstOrDefaultAsync();

            bool isDuplicate = false;
            //check if an existing commitment is changed to match an existing commitment
            if (existingCommitmentID > 0)
                if (model.CommitmentId > 0 && model.CommitmentId != existingCommitmentID) isDuplicate = true;
            //check if a new commitment matches an existing commitment
            if (existingCommitmentID > 0 && model.CommitmentId == 0) isDuplicate = true;
            return isDuplicate;
        }

        public DateTime CalculateEndDate(double ServiceOwed, DateTime startDate)
        {
            int MonthsOwed = Convert.ToInt32(ServiceOwed * 12);
            DateTime EndDate = startDate.AddMonths(MonthsOwed);
            return EndDate;
        }

        public class NextStatusData
        {
            public int StatusID { get; set; }
            public string StatusCode { get; set; }
            public string StatusName { get; set; }
        }

        public class Result
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
            public int ID { get; set; }
        }
    }
}
