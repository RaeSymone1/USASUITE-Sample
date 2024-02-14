using Humanizer;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.SharedCode.StudentDashboardRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Admin.StudentDashboardModel;

namespace OPM.SFS.Tests
{
	[TestClass]
	public class StudentDashboardUpdateTests
	{
		[TestMethod]
		public void StudentDashboard_ContractID_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetContractAsync()).Returns(Task.FromResult(GenerateContractValues()));
			ContractValueRule testMe = new ContractValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			var result = testMe.CalculateDashboardFieldAsync("Version 1", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.ContractId == 1);
		}

		[TestMethod]
		public void StudentDashboard_ContractID_Removed_When_Value_Is_N_A()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetContractAsync()).Returns(Task.FromResult(GenerateContractValues()));
			ContractValueRule testMe = new ContractValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ContractId = 1;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(!recordToUpdate.ContractId.HasValue);
		}

		[TestMethod]
		public void StudentDashboard_Status_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetStatusOptionsAsync()).Returns(Task.FromResult(GenerateStatusOptionValues()));
			StatusValueRule testMe = new StatusValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			var result = testMe.CalculateDashboardFieldAsync("Deferred", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.StatusID == 66);
		}

		[TestMethod]
		public void StudentDashboard_Status_Removed_When_Value_Is_N_A()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetStatusOptionsAsync()).Returns(Task.FromResult(GenerateStatusOptionValues()));
			StatusValueRule testMe = new StatusValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.StatusID = 1;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(!recordToUpdate.StatusID.HasValue);
		}

		[TestMethod]
		public void StudentDashboard_ExtensionType_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			ExtensionTypeValueRule testMe = new ExtensionTypeValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			var result = testMe.CalculateDashboardFieldAsync("No Extension", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.ExtensionTypeID == 4);
		}

		[TestMethod]
		public void StudentDashboard_ExtensionType_Removed_When_Value_Is_N_A()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetStatusOptionsAsync()).Returns(Task.FromResult(GenerateStatusOptionValues()));
			ExtensionTypeValueRule testMe = new ExtensionTypeValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ExtensionTypeID = 4;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(!recordToUpdate.ExtensionTypeID.HasValue);
		}

		[TestMethod]
		public void StudentDashboard_PGEmploymentDueDate_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			var utiltiesMock = new Mock<IUtilitiesService>();
			PGEmploymentDueDateValueRule testMe = new PGEmploymentDueDateValueRule(referenceRepoMock.Object, utiltiesMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ExpectedGradDate = Convert.ToDateTime("8/30/2024");
			var newPGEOD = "9/1/2024";
			var result = testMe.CalculateDashboardFieldAsync(newPGEOD, recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGEmploymentDueDate == Convert.ToDateTime(newPGEOD));
		}

		[TestMethod]
		public void StudentDashboard_PGEmploymentDueDate_AutoCalculated_When_Value_Is_Null_And_Extension_Type_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			var utiltiesMock = new Mock<IUtilitiesService>();
			PGEmploymentDueDateValueRule testMe = new PGEmploymentDueDateValueRule(referenceRepoMock.Object, utiltiesMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ExpectedGradDate = Convert.ToDateTime("8/30/2024");
			recordToUpdate.ExtensionTypeID = 3; //First Extension (6 Months)
			var expectedDate = "8/1/2026";
			var result = testMe.CalculateDashboardFieldAsync("", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGEmploymentDueDate == Convert.ToDateTime(expectedDate));
		}

		[TestMethod]
		public void StudentDashboard_PGEmploymentDueDate_Removed_When_Value_Is_N_A_And_No_ExtenstionType_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			var utiltiesMock = new Mock<IUtilitiesService>();
			PGEmploymentDueDateValueRule testMe = new PGEmploymentDueDateValueRule(referenceRepoMock.Object, utiltiesMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ExtensionTypeID = null;
			recordToUpdate.PGEmploymentDueDate = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(!recordToUpdate.PGEmploymentDueDate.HasValue);
		}


		[TestMethod]
		public void StudentDashboard_DateLeftPGEary_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			DateLeftPGEarlyValueRule testMe = new DateLeftPGEarlyValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var result = testMe.CalculateDashboardFieldAsync("4/5/2024", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.DateLeftPGEarly == Convert.ToDateTime("4/5/2024"));
		}

		[TestMethod]
		public void StudentDashboard_DateLeftPGEary_Removed_When_Value_Is_NA()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			DateLeftPGEarlyValueRule testMe = new DateLeftPGEarlyValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.DateLeftPGEarly = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(!recordToUpdate.DateLeftPGEarly.HasValue);
		}

		[TestMethod]
		public void StudentDashboard_Funding_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			FundingAmountValueRule testMe = new FundingAmountValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var result = testMe.CalculateDashboardFieldAsync("45", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.FundingAmount == Convert.ToDecimal("45"));
		}

		[TestMethod]
		public void StudentDashboard_Funding_Removed_When_Value_Is_NA()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			FundingAmountValueRule testMe = new FundingAmountValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.FundingAmount = 45;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(!recordToUpdate.FundingAmount.HasValue);
		}

		[TestMethod]
		public void StudentDashboard_PGVerificationOneCompleteDate_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			PGVerificationOneCompleteDateValueRule testMe = new PGVerificationOneCompleteDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var result = testMe.CalculateDashboardFieldAsync("4/1/2024", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationOneCompleteDate == Convert.ToDateTime("4/1/2024"));
		}

		[TestMethod]
		public void StudentDashboard_PGVerificationOneCompleteDate_Removed_When_Value_Is_NA()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetExtensionTypeAsync()).Returns(Task.FromResult(GenerateExtensionTypes()));
			PGVerificationOneCompleteDateValueRule testMe = new PGVerificationOneCompleteDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.PGVerificationOneCompleteDate = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(!recordToUpdate.PGVerificationOneCompleteDate.HasValue);
		}

		[TestMethod]
		public void StudentDashboard_ServiceOwed_Updated_When_Session_And_Funding_Is_Set_With_No_TotalTerms()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetInstitutionsAsync()).Returns(Task.FromResult(GenerateInstitutions()));
			IServiceOwedService service = new ServiceOwedService();
			ServiceOwedValueRule testMe = new ServiceOwedValueRule(referenceRepoMock.Object, service);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.EnrolledSession = "Fall";
			recordToUpdate.EnrolledYear = 2024;
			recordToUpdate.FundingEndSession = "Summer";
			recordToUpdate.FundingEndYear = 2026;
			recordToUpdate.InstitutionId = 1;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.ServiceOwed.Value == 2.5);
		}

		[TestMethod]
		public void StudentDashboard_ServiceOwed_Updated_When_TotalTerms_Is_Set_With_No_Funding_Or_Session()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetInstitutionsAsync()).Returns(Task.FromResult(GenerateInstitutions()));
			IServiceOwedService service = new ServiceOwedService();
			ServiceOwedValueRule testMe = new ServiceOwedValueRule(referenceRepoMock.Object, service);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.TotalAcademicTerms = "6";
			recordToUpdate.InstitutionId = 1;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.ServiceOwed.Value == 3);
		}

		[TestMethod]
		public void StudentDashboard_TotalTerms_AutoCalculated_When_Session_And_Funding_Is_Set_And_TotalTerms_Is_Empty()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetInstitutionsAsync()).Returns(Task.FromResult(GenerateInstitutions()));
			TotalTermsValueRule testMe = new TotalTermsValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.EnrolledSession = "Fall";
			recordToUpdate.EnrolledYear = 2023;
			recordToUpdate.FundingEndSession = "Summer";
			recordToUpdate.FundingEndYear = 2026;
			recordToUpdate.InstitutionId = 2;
			var result = testMe.CalculateDashboardFieldAsync("", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.TotalAcademicTerms == "10");
		}

		[TestMethod]
		public void StudentDashboard_TotalTerms_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetInstitutionsAsync()).Returns(Task.FromResult(GenerateInstitutions()));
			TotalTermsValueRule testMe = new TotalTermsValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.EnrolledSession = "Fall";
			recordToUpdate.EnrolledYear = 2023;
			recordToUpdate.FundingEndSession = "Summer";
			recordToUpdate.FundingEndYear = 2026;
			recordToUpdate.InstitutionId = 2;
			var result = testMe.CalculateDashboardFieldAsync("15", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.TotalAcademicTerms == "15");
		}

		[TestMethod]
		public void StudentDashboard_PGVerificationOneDueDate_Updated_When_Value_Is_Set()
		{
			PGVerificationOneDueDateValueRule testMe = new PGVerificationOneDueDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 2;
			recordToUpdate.PostGradEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.PGVerificationOneDueDate = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("1/10/2024", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationOneDueDate == Convert.ToDateTime("1/10/2024"));

		}
		[TestMethod]
		public void StudentDashboard_PGVerificationOneDueDate_AutoCalculated_ServiceOwed_And_PGEOD_Set_And_PGVerificationOneDueDate_Is_Empty()
		{
			PGVerificationOneDueDateValueRule testMe = new PGVerificationOneDueDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 2;
			DateTime pgEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.PostGradEOD = pgEOD;
			recordToUpdate.PGVerificationOneDueDate = DateTime.UtcNow;
			DateTime expectedValue = pgEOD.AddYears(1);
			var result = testMe.CalculateDashboardFieldAsync("", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationOneDueDate == expectedValue);

		}

		[TestMethod]
		public void StudentDashboard_PGVerificationOneDueDate_AutoCalculated_ServiceOwed_And_PGEOD_Set_And_PGVerificationOneDueDate_Is_NA()
		{
			PGVerificationOneDueDateValueRule testMe = new PGVerificationOneDueDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 2;
			DateTime pgEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.PostGradEOD = pgEOD;
			recordToUpdate.PGVerificationOneDueDate = DateTime.UtcNow;
			DateTime expectedValue = pgEOD.AddYears(1);
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationOneDueDate == expectedValue);

		}

		[TestMethod]
		public void StudentDashboard_PGVerificationTwoDueDate_Updated_When_Value_Is_Set()
		{
			PGVerificationTwoDueDateValueRule testMe = new PGVerificationTwoDueDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 2;
			recordToUpdate.PostGradEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.PGVerificationOneDueDate = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("1/10/2024", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationTwoDueDate == Convert.ToDateTime("1/10/2024"));

		}
		[TestMethod]
		public void StudentDashboard_PGVerificationTwoDueDate_AutoCalculated_ServiceOwed_And_PGEOD_Set_And_PGVerificationOneDueDate_Is_Empty()
		{
			PGVerificationTwoDueDateValueRule testMe = new PGVerificationTwoDueDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 3;
			DateTime pgEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.PostGradEOD = pgEOD;
			recordToUpdate.PGVerificationOneDueDate = DateTime.UtcNow;
			DateTime expectedValue = pgEOD.AddYears(2);
			var result = testMe.CalculateDashboardFieldAsync("", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationTwoDueDate == expectedValue);

		}

		[TestMethod]
		public void StudentDashboard_PGVerificationTwoDueDate_AutoCalculated_ServiceOwed_And_PGEOD_Set_And_PGVerificationOneDueDate_Is_Empty_Should_Be_Null()
		{
			PGVerificationTwoDueDateValueRule testMe = new PGVerificationTwoDueDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 2;
			DateTime pgEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.PostGradEOD = pgEOD;
			recordToUpdate.PGVerificationOneDueDate = DateTime.UtcNow;
			DateTime? expectedValue = null;
			var result = testMe.CalculateDashboardFieldAsync("", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationTwoDueDate == expectedValue);

		}

		[TestMethod]
		public void StudentDashboard_PGVerificationTwoDueDate_AutoCalculated_ServiceOwed_And_PGEOD_Set_And_PGVerificationOneDueDate_Is_NA()
		{
			PGVerificationTwoDueDateValueRule testMe = new PGVerificationTwoDueDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 3;
			DateTime pgEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.PostGradEOD = pgEOD;
			recordToUpdate.PGVerificationOneDueDate = DateTime.UtcNow;
			DateTime expectedValue = pgEOD.AddYears(2);
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.PGVerificationTwoDueDate == expectedValue);

		}

		[TestMethod]
		public void StudentDashboard_CommitmentPhaseComplete_Updated_When_Value_Is_Set()
		{
			CommitmentPhaseCompleteValueRule testMe = new CommitmentPhaseCompleteValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 2;
			recordToUpdate.PostGradEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.CommitmentPhaseComplete = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("1/10/2024", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.CommitmentPhaseComplete == Convert.ToDateTime("1/10/2024"));

		}

		[TestMethod]
		public void StudentDashboard_CommitmentPhaseComplete_AutoCalculated_When_PGEOD_AND_ServiceOwed_Set_And_CommitmentPhaseComplete_Is_Empty()
		{
			CommitmentPhaseCompleteValueRule testMe = new CommitmentPhaseCompleteValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.ServiceOwed = 2;
			recordToUpdate.PostGradEOD = Convert.ToDateTime("8/31/2024");
			recordToUpdate.CommitmentPhaseComplete = null;
			var expectedResult = Convert.ToDateTime("8/31/2026");
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.CommitmentPhaseComplete == expectedResult);

		}

		[TestMethod]
		public void StudentDashboard_SOCVerificationComplete_Updated_When_Value_Is_Set()
		{
			SOCVerificationCompleteValueRule testMe = new SOCVerificationCompleteValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var expectedResult = Convert.ToDateTime("8/31/2026");
			var result = testMe.CalculateDashboardFieldAsync(expectedResult.ToShortDateString(), recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.SOCVerificationComplete == expectedResult);
		}

		[TestMethod]
		public void StudentDashboard_SOCVerificationComplete_Removed_When_Value_Is_Empty()
		{
			SOCVerificationCompleteValueRule testMe = new SOCVerificationCompleteValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.SOCVerificationComplete = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.SOCVerificationComplete == null);
		}

		[TestMethod]
		public void StudentDashboard_FollowUpDate_Updated_When_Value_Is_Set()
		{
			FollowUpDateValueRule testMe = new FollowUpDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var expectedResult = Convert.ToDateTime("8/31/2026");
			var result = testMe.CalculateDashboardFieldAsync(expectedResult.ToShortDateString(), recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.FollowUpDate == expectedResult);
		}

		[TestMethod]
		public void StudentDashboard_FollowUpDate_Removed_When_Value_Is_Empty()
		{
			FollowUpDateValueRule testMe = new FollowUpDateValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.FollowUpDate = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.FollowUpDate == null);
		}

		[TestMethod]
		public void StudentDashboard_FollowUpAction_Updated_When_Value_Is_Set()
		{
			FollowUpActionValueRule testMe = new FollowUpActionValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var expectedResult = "Follow up now!";
			var result = testMe.CalculateDashboardFieldAsync(expectedResult, recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.FollowUpAction == expectedResult);
		}

		[TestMethod]
		public void StudentDashboard_FollowUpAction_Removed_When_Value_Is_Empty()
		{
			FollowUpActionValueRule testMe = new FollowUpActionValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.FollowUpAction = "Follow up now!!";
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.FollowUpAction == null);
		}
		[TestMethod]
		public void StudentDashboard_FollowUpTypeOption_Updated_When_Value_Is_Set()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetFollowUpTypeOptionsAsync()).Returns(Task.FromResult(GenerateFollowUpTypes()));
			FollowUpTypeOptionValueRule testMe = new FollowUpTypeOptionValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			var expectedResult = 1;
			var result = testMe.CalculateDashboardFieldAsync("Repayment", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.FollowUpTypeOptionID == expectedResult);
		}

		[TestMethod]
		public void StudentDashboard_FollowUpTypeOption_Removed_When_Value_Is_Empty()
		{
			var referenceRepoMock = new Mock<IReferenceDataRepository>();
			referenceRepoMock.Setup(m => m.GetFollowUpTypeOptionsAsync()).Returns(Task.FromResult(GenerateFollowUpTypes()));
			FollowUpTypeOptionValueRule testMe = new FollowUpTypeOptionValueRule(referenceRepoMock.Object);
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.FollowUpTypeOptionID = 1;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.FollowUpTypeOptionID == null);
		}

		[TestMethod]
		public void StudentDashboard_DateReleasedCollectionPackage_Updated_When_Value_Is_Set()
		{
			DateReleasedCollectionPackageValueRule testMe = new DateReleasedCollectionPackageValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var expectedResult = Convert.ToDateTime("8/31/2026");
			var result = testMe.CalculateDashboardFieldAsync(expectedResult.ToShortDateString(), recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.DateReleasedCollectionPackage == expectedResult);
		}

		[TestMethod]
		public void StudentDashboard_DateReleasedCollectionPackage_Removed_When_Value_Is_Empty()
		{
			DateReleasedCollectionPackageValueRule testMe = new DateReleasedCollectionPackageValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.DateReleasedCollectionPackage = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.DateReleasedCollectionPackage == null);
		}

		[TestMethod]
		public void StudentDashboard_DatePendingReleaseCollectionInfo_Updated_When_Value_Is_Set()
		{
			DatePendingReleaseCollectionInfoValueRule testMe = new DatePendingReleaseCollectionInfoValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			var expectedResult = Convert.ToDateTime("8/31/2026");
			var result = testMe.CalculateDashboardFieldAsync(expectedResult.ToShortDateString(), recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.DatePendingReleaseCollectionInfo == expectedResult);
		}

		[TestMethod]
		public void StudentDashboard_DatePendingReleaseCollectionInfo_Removed_When_Value_Is_Empty()
		{
			DatePendingReleaseCollectionInfoValueRule testMe = new DatePendingReleaseCollectionInfoValueRule();
			var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.DatePendingReleaseCollectionInfo = DateTime.UtcNow;
			var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
			Assert.IsTrue(result);
			Assert.IsTrue(recordToUpdate.DatePendingReleaseCollectionInfo == null);
		}

        [TestMethod]
        public void StudentDashboard_Notes_Updated_When_Value_Is_Set()
        {
            NotesValueRule testMe = new NotesValueRule();
            var recordToUpdate = new StudentInstitutionFunding();
			var expectedResult = "Hello world!";
            var result = testMe.CalculateDashboardFieldAsync(expectedResult, recordToUpdate).Result;
            Assert.IsTrue(result);
            Assert.IsTrue(recordToUpdate.Notes == expectedResult);
        }

        [TestMethod]
        public void StudentDashboard_Notes_Removed_When_Value_Is_Empty()
        {
            NotesValueRule testMe = new NotesValueRule();
            var recordToUpdate = new StudentInstitutionFunding();
			recordToUpdate.Notes = "Hello World";
            var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
            Assert.IsTrue(result);
            Assert.IsTrue(recordToUpdate.Notes == null);
        }

        [TestMethod]
        public void StudentDashboard_LastUpdated_Updated_When_Value_Is_Set()
        {
            LastUpdateReceivedValueRule testMe = new LastUpdateReceivedValueRule();
            var recordToUpdate = new Student();
            var expectedResult = Convert.ToDateTime("8/31/2026");
            var result = testMe.CalculateDashboardFieldAsync(expectedResult.ToShortDateString(), recordToUpdate).Result;
            Assert.IsTrue(result);
            Assert.IsTrue(recordToUpdate.LastUpdated == expectedResult);
        }

        [TestMethod]
        public void StudentDashboard_LastUpdated_Removed_When_Value_Is_Empty()
        {
            LastUpdateReceivedValueRule testMe = new LastUpdateReceivedValueRule();
            var recordToUpdate = new Student();
            recordToUpdate.LastUpdated = DateTime.UtcNow;
            var result = testMe.CalculateDashboardFieldAsync("N/A", recordToUpdate).Result;
            Assert.IsTrue(result);
            Assert.IsTrue(recordToUpdate.LastUpdated == null);
        }

        private List<Data.Contract> GenerateContractValues()
		{
			var results = new List<Data.Contract>();
			results.Add(new Data.Contract() { ContractId = 1, Name = "Version 1" });
			results.Add(new Data.Contract() { ContractId = 2, Name = "Version 2" });
			results.Add(new Data.Contract() { ContractId = 3, Name = "Version 3" });
			return results;
		}

		private List<Data.Institution> GenerateInstitutions()
		{
			var results = new List<Data.Institution>();
			AcademicSchedule schedule = new AcademicSchedule();
			schedule.AcademicScheduleId = 1;
			schedule.Name = "Semester";
			schedule.Code = "1";

			AcademicSchedule schedule2 = new AcademicSchedule();
			schedule2.AcademicScheduleId = 2;
			schedule2.Name = "Quarter";
			schedule2.Code = "2";

			results.Add(new Data.Institution() { InstitutionId = 1, Name = "University of Georgia", AcademicSchedule = schedule });
			results.Add(new Data.Institution() { InstitutionId = 2, Name = "Oregon State University", AcademicSchedule = schedule2 });
			return results;
		}

		private List<Data.StatusOption> GenerateStatusOptionValues()
		{
			var results = new List<Data.StatusOption>();
			results.Add(new Data.StatusOption() { StudentStatusId = 66, Option = "Deferred", Status = "Deferred", Phase = "Deferred", PostGradPlacementGroup = "Still in School" });
			results.Add(new Data.StatusOption() { StudentStatusId = 70, Option = "Meeting", Status = "NG meeting service commitment", Phase = "Employment", PostGradPlacementGroup = "Placed" });
			results.Add(new Data.StatusOption() { StudentStatusId = 92, Option = "Pending", Status = "Release NonGrad - Pending", Phase = "Release", PostGradPlacementGroup = "Non Grad Release" });
			return results;
		}

		private List<ExtensionType> GenerateExtensionTypes()
		{
			var results = new List<ExtensionType>();
			ExtensionType _one = new ExtensionType() { ExtensionTypeID = 1, Extension = "First Extension (12 Months)", Months = 12 };
			ExtensionType _two = new ExtensionType() { ExtensionTypeID = 2, Extension = "First Extension (9 Months)", Months = 9 };
			ExtensionType _three = new ExtensionType() { ExtensionTypeID = 3, Extension = "First Extension (6 Months)", Months = 6 };
			ExtensionType _four = new ExtensionType() { ExtensionTypeID = 4, Extension = "No Extension", Months = 0 };
			ExtensionType _five = new ExtensionType() { ExtensionTypeID = 5, Extension = "Second Extension (6 + 12 Months)", Months = 18 };
			ExtensionType _six = new ExtensionType() { ExtensionTypeID = 6, Extension = "Second Extension (6 + 9 Months)", Months = 15 };
			ExtensionType _seven = new ExtensionType() { ExtensionTypeID = 7, Extension = "Second Extension (6 + 6 Months)", Months = 12 };
			results.Add(_one);
			results.Add(_two);
			results.Add(_three);
			results.Add(_four);
			results.Add(_five);
			results.Add(_six);
			results.Add(_seven);
			return results;
			
		}

		private List<FollowUpTypeOption> GenerateFollowUpTypes()
		{
			List<FollowUpTypeOption> list = new List<FollowUpTypeOption>();
			list.Add(new FollowUpTypeOption() { FollowUpTypeOptionID = 1, Name = "Repayment" });
			list.Add(new FollowUpTypeOption() { FollowUpTypeOptionID = 2, Name = "Waiver Release System" });
			return list;
		}

		private SFS.Data.Student GenerateStudentRecord()
		{
			SFS.Data.Student student = new SFS.Data.Student();
			student.StudentUID = 123;
			student.StudentId = 1;
			student.FirstName = "Test";
			student.LastName = "Testing";			
			return student;
		}

		private SFS.Data.StudentInstitutionFunding GenerateFundingRecord()
		{
			SFS.Data.StudentInstitutionFunding funding = new();
			funding.StudentId = 1;
			funding.InstitutionId = 1;
			funding.ExpectedGradDate = DateTime.UtcNow;
			return funding;
		}

		private SFS.Data.StudentInstitutionFunding GenerateFundingWithVerifyDatesRecord()
		{
			SFS.Data.StudentInstitutionFunding funding = new();
			funding.StudentId = 1;
			funding.InstitutionId = 1;
			funding.ExpectedGradDate = DateTime.UtcNow;
			funding.PGVerificationOneDueDate = Convert.ToDateTime("12/1/2010");
			funding.PGVerificationTwoDueDate = Convert.ToDateTime("12/1/2011");
			return funding;
		}

		private ServiceOwed GenerateServiceOwed()
		{
			return new ServiceOwed() { ServiceTime = 3, ex = ""};
		}
	}
}
