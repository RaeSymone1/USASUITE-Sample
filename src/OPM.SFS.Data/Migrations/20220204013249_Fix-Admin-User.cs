using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class FixAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminUserPasswordHistory_AdminUser_AdminUserID",
                table: "AdminUserPasswordHistory");

            migrationBuilder.DropTable(
                name: "AdminUser");

            migrationBuilder.DropIndex(
                name: "IX_AdminUserPasswordHistory_AdminUserID",
                table: "AdminUserPasswordHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUser",
                columns: table => new
                {
                    AdminUserID = table.Column<int>(type: "int", nullable: false),
                    AdminUserRoleID = table.Column<int>(type: "int", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: true),
                    FailedLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockedOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PasswordExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneExt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.AdminUserID);
                    table.ForeignKey(
                        name: "FK_AdminUser_AdminUserRoles_AdminUserRoleID",
                        column: x => x.AdminUserRoleID,
                        principalTable: "AdminUserRoles",
                        principalColumn: "AdminUserRoleID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUserPasswordHistory_AdminUserID",
                table: "AdminUserPasswordHistory",
                column: "AdminUserID");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_AdminUserRoleID",
                table: "AdminUser",
                column: "AdminUserRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminUserPasswordHistory_AdminUser_AdminUserID",
                table: "AdminUserPasswordHistory",
                column: "AdminUserID",
                principalTable: "AdminUser",
                principalColumn: "AdminUserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
