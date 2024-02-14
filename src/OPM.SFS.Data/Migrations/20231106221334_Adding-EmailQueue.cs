using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingEmailQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailQueue",
                columns: table => new
                {
                    EmailQueueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriorityID = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "varchar(50)", nullable: true),
                    FromUri = table.Column<string>(type: "varchar(128)", nullable: true),
                    ToUri = table.Column<string>(type: "varchar(128)", nullable: true),
                    Subject = table.Column<string>(type: "varchar(300)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMultiPart = table.Column<bool>(type: "bit", nullable: false),
                    ReplyTo = table.Column<string>(type: "varchar(128)", nullable: true),
                    Sender = table.Column<string>(type: "varchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueue", x => x.EmailQueueID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailQueue");
        }
    }
}
