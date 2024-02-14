using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class CommitmentReject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectNote",
                table: "StudentCommitment",
                type: "nvarchar(1000)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_StateID",
                table: "Institution",
                column: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_State_StateID",
                table: "Institution",
                column: "StateID",
                principalTable: "State",
                principalColumn: "StateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_State_StateID",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_StateID",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "RejectNote",
                table: "StudentCommitment");
        }
    }
}
