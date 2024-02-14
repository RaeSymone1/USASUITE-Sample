using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSPAddFundingRowID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			var sp = @"ALTER PROCEDURE [dbo].[GetAllStudentsForDashboard] 
						AS
						BEGIN
							select StudentUID, s.StudentID, f.StudentInstitutionFundingId,Firstname, LastName, i.Name as Institution, i.InstitutionID, it.Name as SchoolType, ac.Name as AcademicSchedule,
							f.EnrolledSession, f.EnrolledYear, f.FundingEndSession, f.FundingEndYear, f.FundingAmount, f.ExpectedGradDate as GraduationDate, f.TotalAcademicTerms,
							f.DateLeftPGEarly, st.[Option] as StatusOption, Phase as ProgramPhase, st.PostGradPlacementGroup as PlacementCategory, st.Status,
							p.Name as ProfileStatus, s.Email as EmailAddress, d.Name as Degree, major.Name as Major, minor.Name as Minor, ddmajor.Name as SecondDegreeMajor, ddminor.Name as SecondDegreeMinor,
							s.AlternateEmail as AltEmail, et.Extension as ExtentionType, et.Months as ExtensionMonths, FORMAT(f.PGEmploymentDueDate,'M/d/yyyy') as PGEmploymentDueDate, 
							Format(f.PGVerificationOneDueDate, 'M/d/yyyy') as PGVerificationOneDueDate, Format(f.PGVerificationTwoDueDate, 'M/d/yyyy') as PGVerificationTwoDueDate, 
							Format(f.PGVerificationOneCompleteDate, 'M/d/yyyy') as PGVerificationOneCompleteDate, Format(f.PGVerificationTwoCompleteDate, 'M/d/yyyy') as PGVerificationTwoCompleteDate, 
							Format(f.CommitmentPhaseComplete, 'M/d/yyyy') as CommitmentPhaseComplete, Format(f.SOCVerificationComplete, 'M/d/yyyy') as SOCVerificationComplete, c.Value as Citizenship,
							s.LastUpdated, con.Name as Contract, f.ServiceOwed, f.InternshipReported, f.PostGradReported, f.InternshipAgencyType, f.InternshipAgencyName, f.InternshipSubAgencyName, f.InternshipEOD,
							f.AdditionalInternshipAgencyType, f.AdditionalInternshipAgencyName, f.AdditionalInternshipSubAgencyName, f.AdditionalInternshipReportedWebsite, 
							f.PostGradAgencyType, f.PostGradAgencyName, f.AdditionalPostGradSubAgencyName, f.PostGradEOD, f.AdditionalPostGradAgencyType, f.AdditionalPostGradAgencyName,
							f.AdditionalPostGradSubAgencyName, f.AdditionalPostGradReportedWebsite, f.Notes, Format(f.FollowUpDate, 'M/d/yyyy') as FollowUpDate, f.FollowUpAction, follow.Name as FollowUpType, 
							Format(f.DatePendingReleaseCollectionInfo, 'M/d/yyyy') as DatePendingReleaseCollectionInfo, Format(f.DateReleasedCollectionPackage, 'M/d/yyyy') as DateReleasedCollectionPackage
							from Student s
							inner join StudentInstitutionFundings f on s.StudentID = f.StudentId 
							left outer join StatusOption st on st.StudentStatusID = f.statusid 
							inner join ProfileStatus p on p.ProfileStatusID = s.ProfileStatusID
							inner join Institution i on i.InstitutionID = f.InstitutionId
							inner join InstitutionType it on it.InstitutionTypeID = i.InstitutionTypeID
							inner join AcademicSchedule ac on ac.AcademicScheduleId = i.AcademicScheduleId
							inner join Degree d on d.DegreeID = f.DegreeId 
							inner join Discipline major on major.DisciplineID = f.MajorId
							left outer join Discipline minor on minor.DisciplineID = f.MinorId
							left outer join Discipline ddmajor on ddmajor.DisciplineID  = f.SecondDegreeMajorId
							left outer join Discipline ddminor on ddminor.DisciplineID = f.SecondDegreeMinorId
							left outer join ExtensionType et on et.ExtensionTypeID = f.ExtensionTypeID
							left outer join Citizenship c on c.CitizenshipID = s.CitizenshipID
							left outer join Contract con on con.ContractId = f.ContractId
							left outer join FollowUpTypeOption follow on follow.FollowUpTypeOptionID = f.FollowUpTypeOptionID
							order by StudentUID
						END
						GO";
			migrationBuilder.Sql(sp);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
