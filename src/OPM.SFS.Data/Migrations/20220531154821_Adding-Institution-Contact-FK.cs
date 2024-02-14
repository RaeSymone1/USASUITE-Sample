using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingInstitutionContactFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InstitutionContact_InstitutionId",
                table: "InstitutionContact",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionContact_Institution_InstitutionId",
                table: "InstitutionContact",
                column: "InstitutionId",
                principalTable: "Institution",
                principalColumn: "InstitutionID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionContact_Institution_InstitutionId",
                table: "InstitutionContact");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionContact_InstitutionId",
                table: "InstitutionContact");
        }
    }
}
