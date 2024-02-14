using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class FixingEducation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstitutionTypeID",
                table: "Education",
                newName: "SchoolTypeID");

            migrationBuilder.AddColumn<string>(
                name: "CreditTypeOther",
                table: "Education",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Education_SchoolTypeID",
                table: "Education",
                column: "SchoolTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_SchoolType_SchoolTypeID",
                table: "Education",
                column: "SchoolTypeID",
                principalTable: "SchoolType",
                principalColumn: "SchoolTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_SchoolType_SchoolTypeID",
                table: "Education");

            migrationBuilder.DropIndex(
                name: "IX_Education_SchoolTypeID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "CreditTypeOther",
                table: "Education");

            migrationBuilder.RenameColumn(
                name: "SchoolTypeID",
                table: "Education",
                newName: "InstitutionTypeID");
        }
    }
}
