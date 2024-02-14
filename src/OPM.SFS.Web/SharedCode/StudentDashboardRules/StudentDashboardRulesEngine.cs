using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public interface IStudentDashboardRulesEngine
	{
		Task BuildUpdateAsync(StudentInstitutionFunding recordToUpdate, Student studentRecordToUpdate, StudentDashboardViewModel values);
	}

	public class StudentDashboardRulesEngine : IStudentDashboardRulesEngine
	{
		private readonly IReferenceDataRepository _repo;
		private readonly IServiceOwedService _serviceowed;
		private readonly IUtilitiesService _utilities;
		public StudentDashboardRulesEngine(IReferenceDataRepository repo, IServiceOwedService serviceowed, IUtilitiesService utilities)
		{
			_repo = repo;
			_serviceowed = serviceowed;
			_utilities = utilities;
		}

		public async Task BuildUpdateAsync(StudentInstitutionFunding recordToUpdate, Student studentRecordToUpdate, StudentDashboardViewModel values)
		{
			
			CommitmentPhaseCompleteValueRule _commitPhaseCompleteUpdate = new CommitmentPhaseCompleteValueRule();
			ContractValueRule _contractUpdate = new ContractValueRule(_repo);
			DateLeftPGEarlyValueRule _dateLeftPGEarlyUpdate = new DateLeftPGEarlyValueRule();
			DatePendingReleaseCollectionInfoValueRule _datePendingReleaseUpdate = new DatePendingReleaseCollectionInfoValueRule();
			DateReleasedCollectionPackageValueRule _dateReleaseCollection = new DateReleasedCollectionPackageValueRule();
			ExtensionTypeValueRule _extensionUpdate = new ExtensionTypeValueRule(_repo);
			FollowUpActionValueRule _followupActionUpdate = new FollowUpActionValueRule();
			FollowUpDateValueRule _followupDateUpdate = new FollowUpDateValueRule();
			FollowUpTypeOptionValueRule _followUpTypeUpdate = new FollowUpTypeOptionValueRule(_repo);
			FundingAmountValueRule _fundingUpdate = new FundingAmountValueRule();
			PGEmploymentDueDateValueRule _pgEmployDueDateUpdate = new PGEmploymentDueDateValueRule(_repo, _utilities);
			PGVerificationOneCompleteDateValueRule _pgVerifyOneCompleteUpdate = new PGVerificationOneCompleteDateValueRule();
			PGVerificationOneDueDateValueRule _pgVerifyOneDueUpdate = new PGVerificationOneDueDateValueRule();
			PGVerificationTwoCompleteDateValueRule _pgVerifyTwoCompleteUpdate = new PGVerificationTwoCompleteDateValueRule();
			PGVerificationTwoDueDateValueRule _pgVerifyTwoDueUpdate = new PGVerificationTwoDueDateValueRule();
			ServiceOwedValueRule _servieOwedUpdate = new ServiceOwedValueRule(_repo, _serviceowed);
			SOCVerificationCompleteValueRule _SOCVerifyUpdate = new SOCVerificationCompleteValueRule();
			StatusValueRule _statusUpdate = new StatusValueRule(_repo);
			TotalTermsValueRule _totalTermsUpdate = new TotalTermsValueRule(_repo);
			CitizenshipValueRule _citizenshipUpdate = new CitizenshipValueRule(_repo);
			NotesValueRule _notesUpdate = new NotesValueRule();
			LastUpdateReceivedValueRule _lastUpdated = new LastUpdateReceivedValueRule();

			await _totalTermsUpdate.CalculateDashboardFieldAsync(values.TotalAcademicTerms, recordToUpdate);
			await _servieOwedUpdate.CalculateDashboardFieldAsync("", recordToUpdate); //pass in empty string to autocalculate Service Owed
			await _extensionUpdate.CalculateDashboardFieldAsync(values.ExtensionTypes, recordToUpdate);
			await _pgEmployDueDateUpdate.CalculateDashboardFieldAsync(values.PGEmploymentDueDate, recordToUpdate);
			await _statusUpdate.CalculateDashboardFieldAsync(values.Status, recordToUpdate);
			await _commitPhaseCompleteUpdate.CalculateDashboardFieldAsync(values.CommitmentPhaseComplete, recordToUpdate);
			await _contractUpdate.CalculateDashboardFieldAsync(values.Contract, recordToUpdate);
			await _dateLeftPGEarlyUpdate.CalculateDashboardFieldAsync(values.DateLeftPGEarly, recordToUpdate);
			await _datePendingReleaseUpdate.CalculateDashboardFieldAsync(values.ReleasePackageDueDate, recordToUpdate);
			await _dateReleaseCollection.CalculateDashboardFieldAsync(values.ReleasePackageSent, recordToUpdate);			
			await _followupActionUpdate.CalculateDashboardFieldAsync(values.FollowupAction, recordToUpdate);
			await _followupDateUpdate.CalculateDashboardFieldAsync(values.FollowupDate, recordToUpdate);
			await _followUpTypeUpdate.CalculateDashboardFieldAsync(values.FollowupActionType, recordToUpdate);
			await _fundingUpdate.CalculateDashboardFieldAsync(values.Amount, recordToUpdate);			
			await _pgVerifyOneCompleteUpdate.CalculateDashboardFieldAsync(values.PGVerificationOneComplete, recordToUpdate);
			await _pgVerifyOneDueUpdate.CalculateDashboardFieldAsync(values.PGVerificationOneDue, recordToUpdate);
			await _pgVerifyTwoCompleteUpdate.CalculateDashboardFieldAsync(values.PGVerificationTwoComplete, recordToUpdate);
			await _pgVerifyTwoDueUpdate.CalculateDashboardFieldAsync(values.PGVerificationTwoDue, recordToUpdate);			
			await _SOCVerifyUpdate.CalculateDashboardFieldAsync(values.SOCVerificationComplete, recordToUpdate);
			await _citizenshipUpdate.CalculateDashboardFieldAsync(values.Citizenship, studentRecordToUpdate);
			await _notesUpdate.CalculateDashboardFieldAsync(values.StudentNote, recordToUpdate);
			await _lastUpdated.CalculateDashboardFieldAsync(values.LastUpdateReceived, studentRecordToUpdate);
			


		}
	}
}
