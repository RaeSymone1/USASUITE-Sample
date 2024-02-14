using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixingContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contract",
                table: "Student");

			migrationBuilder.AddPrimaryKey(
			  name: "PK_Contract",
			  table: "Contract", column: "ContractId");

			migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "StudentInstitutionFundings",
                type: "int",
                nullable: true);          

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_ContractId",
                table: "StudentInstitutionFundings",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInstitutionFundings_Contract_ContractId",
                table: "StudentInstitutionFundings",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentInstitutionFundings_Contract_ContractId",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropIndex(
                name: "IX_StudentInstitutionFundings_ContractId",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "StudentInstitutionFundings");

            migrationBuilder.AddColumn<string>(
                name: "Contract",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
