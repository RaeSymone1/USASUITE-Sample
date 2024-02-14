using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingJobActivitiy2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentJobActivity_AgencyType_AgencyTypeId",
                table: "StudentJobActivity");

            migrationBuilder.AlterColumn<int>(
                name: "AgencyTypeId",
                table: "StudentJobActivity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJobActivity_AgencyType_AgencyTypeId",
                table: "StudentJobActivity",
                column: "AgencyTypeId",
                principalTable: "AgencyType",
                principalColumn: "AgencyTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentJobActivity_AgencyType_AgencyTypeId",
                table: "StudentJobActivity");

            migrationBuilder.AlterColumn<int>(
                name: "AgencyTypeId",
                table: "StudentJobActivity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJobActivity_AgencyType_AgencyTypeId",
                table: "StudentJobActivity",
                column: "AgencyTypeId",
                principalTable: "AgencyType",
                principalColumn: "AgencyTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
