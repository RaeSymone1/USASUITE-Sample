using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingJobActivityStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentStatus",
                table: "StudentJobActivity",
                newName: "StatusOther");

            migrationBuilder.AddColumn<int>(
                name: "JobActivityStatusID",
                table: "StudentJobActivity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobActivityStatus",
                columns: table => new
                {
                    JobActivityStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true),
                    Code = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobActivityStatus", x => x.JobActivityStatusID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentJobActivity_JobActivityStatusID",
                table: "StudentJobActivity",
                column: "JobActivityStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJobActivity_JobActivityStatus_JobActivityStatusID",
                table: "StudentJobActivity",
                column: "JobActivityStatusID",
                principalTable: "JobActivityStatus",
                principalColumn: "JobActivityStatusID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentJobActivity_JobActivityStatus_JobActivityStatusID",
                table: "StudentJobActivity");

            migrationBuilder.DropTable(
                name: "JobActivityStatus");

            migrationBuilder.DropIndex(
                name: "IX_StudentJobActivity_JobActivityStatusID",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "JobActivityStatusID",
                table: "StudentJobActivity");

            migrationBuilder.RenameColumn(
                name: "StatusOther",
                table: "StudentJobActivity",
                newName: "CurrentStatus");
        }
    }
}
