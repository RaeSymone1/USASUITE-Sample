using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingAdminStudentProfileAndDashboardDataTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionList",
                columns: table => new
                {
                    SessionListId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                });

            migrationBuilder.CreateTable(
           name: "ProgramYear",
           columns: table => new
           {
               ProgramYearId = table.Column<int>(type: "int", nullable: false)
               .Annotation("SqlServer:Identity", "1, 1"),
               Name = table.Column<int>(type: "int", nullable: false)
           });

            migrationBuilder.CreateTable(
          name: "Contract",
          columns: table => new
          {
              ContractId = table.Column<int>(type: "int", nullable: false)
              .Annotation("SqlServer:Identity", "1, 1"),
              Name = table.Column<string>(type: "varchar(100)", nullable: false)
          });

            migrationBuilder.CreateTable(
            name: "FollowupType",
            columns: table => new
            {
                FollowupTypeId = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "varchar(100)", nullable: false)
            });


            migrationBuilder.Sql(@"
                INSERT INTO SessionList(Name)
                VALUES('Winter')

                INSERT INTO SessionList(Name)
                VALUES('Spring')

                INSERT INTO SessionList(Name)
                VALUES('Summer')

                INSERT INTO SessionList(Name)
                VALUES('Fall')

                INSERT INTO Contract(Name)
                VALUES('Version 1')

                INSERT INTO Contract(Name)
                VALUES('Version 2')

                INSERT INTO Contract(Name)
                VALUES('Version 3')

                INSERT INTO FollowupType(Name)
                VALUES('Commitment- Reporting')

                INSERT INTO FollowupType(Name)
                VALUES('Deferral')

                INSERT INTO FollowupType(Name)
                VALUES('Employment Verification Form')

                INSERT INTO FollowupType(Name)
                VALUES('Job Search Status')

                INSERT INTO FollowupType(Name)
                VALUES('Others')

                INSERT INTO FollowupType(Name)
                VALUES('Repayment')

                INSERT INTO FollowupType(Name)
                VALUES('Waiver Release System')


            ");

            StringBuilder mysql = new StringBuilder();
            for (int i = 1970; i <= 2069; i++)
            {
                mysql.AppendLine("INSERT INTO ProgramYear(Name) VALUES(" + i.ToString() + ") ");
            }

            migrationBuilder.Sql(mysql.ToString());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionList"
                );

            migrationBuilder.DropTable(
               name: "ProgramYear"
               );

            migrationBuilder.DropTable(
               name: "Contract"
               );

            migrationBuilder.DropTable(
               name: "FollowupType"
               );
        }
    }
}
