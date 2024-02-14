using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class FixingEducationFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_SchoolType_SchoolTypeID",
                table: "Education");

            migrationBuilder.AlterColumn<int>(
                name: "SchoolTypeID",
                table: "Education",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_SchoolType_SchoolTypeID",
                table: "Education",
                column: "SchoolTypeID",
                principalTable: "SchoolType",
                principalColumn: "SchoolTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_SchoolType_SchoolTypeID",
                table: "Education");

            migrationBuilder.AlterColumn<int>(
                name: "SchoolTypeID",
                table: "Education",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_SchoolType_SchoolTypeID",
                table: "Education",
                column: "SchoolTypeID",
                principalTable: "SchoolType",
                principalColumn: "SchoolTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
