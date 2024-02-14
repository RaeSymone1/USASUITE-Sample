using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingPositionEndDateEmploymentVerification : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
			 name: "PositionEndDate",
			 table: "EmploymentVerification",
			 type: "datetime2",
			 nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
			 name: "PositionEndDate",
			 table: "EmploymentVerification");
		}
    }
}
