using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class InstitutionTableUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_Institution",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_ParentInstitutionID",
                table: "Institution");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Institution_ParentInstitutionID",
                table: "Institution",
                column: "ParentInstitutionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_Institution",
                table: "Institution",
                column: "ParentInstitutionID",
                principalTable: "Institution",
                principalColumn: "InstitutionID");
        }
    }
}
