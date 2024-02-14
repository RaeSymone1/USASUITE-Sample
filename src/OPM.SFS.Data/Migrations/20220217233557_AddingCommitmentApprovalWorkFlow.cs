using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingCommitmentApprovalWorkFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommitmentApprovalWorkflowId",
                table: "Agency",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommitmentApprovalWorkflow",
                columns: table => new
                {
                    CommitmentApprovalWorkflowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentApprovalWorkflow", x => x.CommitmentApprovalWorkflowId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agency_CommitmentApprovalWorkflowId",
                table: "Agency",
                column: "CommitmentApprovalWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_CommitmentApprovalWorkflow_CommitmentApprovalWorkflowId",
                table: "Agency",
                column: "CommitmentApprovalWorkflowId",
                principalTable: "CommitmentApprovalWorkflow",
                principalColumn: "CommitmentApprovalWorkflowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agency_CommitmentApprovalWorkflow_CommitmentApprovalWorkflowId",
                table: "Agency");

            migrationBuilder.DropTable(
                name: "CommitmentApprovalWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_Agency_CommitmentApprovalWorkflowId",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "CommitmentApprovalWorkflowId",
                table: "Agency");
        }
    }
}
