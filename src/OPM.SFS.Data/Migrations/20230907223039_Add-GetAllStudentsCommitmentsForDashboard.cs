using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddGetAllStudentsCommitmentsForDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE OR ALTER   PROCEDURE [dbo].[GetAllStudentsCommitmentsForDashboard]
						@StudentID int = null
						AS
						IF @StudentID IS NULL
							BEGIN
								select StudentId, sc.StudentCommitmentID, a.Name as Agency, a.ParentAgencyID, isnull(parent.Name, 'N/A') as ParentAgency,
								at.Name as AgencyType, StartDate, stat.AdminDisplay as 'Status', DateApproved, tp.Name as CommitmentType
								from StudentCommitment sc (nolock)
								inner join agency a (nolock)
								on sc.AgencyID = a.AgencyID
								left outer join agency parent (nolock) on a.ParentAgencyID = parent.AgencyID
								inner join AgencyType at (nolock) on a.AgencyTypeID = at.AgencyTypeID
								inner join CommitmentStatus stat (nolock) on sc.CommitmentStatusId = stat.CommitmentStatusID
								inner join CommitmentType tp (nolock) on tp.CommitmentTypeID = sc.CommitmentTypeID
								where a.IsDisabled = 0
								and stat.Code <> 'Incomplete' and stat.Code <> 'POReject'
							END
						ELSE 
							BEGIN
								select StudentId, sc.StudentCommitmentID, a.Name as Agency, a.ParentAgencyID, isnull(parent.Name, 'N/A') as ParentAgency,
								at.Name as AgencyType, StartDate, stat.AdminDisplay as 'Status', DateApproved, tp.Name as CommitmentType
								from StudentCommitment sc (nolock)
								inner join agency a (nolock)
								on sc.AgencyID = a.AgencyID
								left outer join agency parent (nolock) on a.ParentAgencyID = parent.AgencyID
								inner join AgencyType at (nolock) on a.AgencyTypeID = at.AgencyTypeID
								inner join CommitmentStatus stat (nolock) on sc.CommitmentStatusId = stat.CommitmentStatusID
								inner join CommitmentType tp (nolock) on tp.CommitmentTypeID = sc.CommitmentTypeID
								where a.IsDisabled = 0
								and stat.Code <> 'Incomplete' and stat.Code <> 'POReject'
								and StudentID = @StudentID
							END
						GO
						GRANT EXEC ON ScholarshipforService.dbo.GetAllStudentsCommitmentsForDashboard TO PUBLIC";
            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
