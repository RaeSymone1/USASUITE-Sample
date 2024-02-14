using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingStudentInstutionFundingsSOCVerificationComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<string>(
            	name: "SOCVerificationComplete",
	            table: "StudentInstitutionFundings",
	            type: "datetime2",
	            nullable: true);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
            	name: "SOCVerificationComplete",
	            table: "StudentInstitutionFundings");
		}
    }
}
