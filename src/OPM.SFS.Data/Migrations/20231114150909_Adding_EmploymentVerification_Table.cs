using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingEmploymentVerificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmploymentVerification",
                columns: table => new
                {
                    EmploymentVerificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    IsSameEmployer = table.Column<bool>(type: "bit", nullable: false),
                    IsSamePosition = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedLadderPromotion = table.Column<bool>(type: "bit", nullable: false),
                    TakingRemedialTraining = table.Column<bool>(type: "bit", nullable: false),
                    Training = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasNewCommitment = table.Column<bool>(type: "bit", nullable: false),
                    EVFDocumentId = table.Column<int>(type: "int", nullable: false),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentVerification", x => x.EmploymentVerificationID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmploymentVerification");
        }
    }
}
