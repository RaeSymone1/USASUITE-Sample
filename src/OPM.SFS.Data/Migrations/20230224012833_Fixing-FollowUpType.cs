using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixingFollowUpType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Contract",
            //    columns: table => new
            //    {
            //        ContractId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Contract", x => x.ContractId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "FollowupType",
            //    columns: table => new
            //    {
            //        FollowupTypeId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FollowupType", x => x.FollowupTypeId);
            //    });

            migrationBuilder.CreateTable(
                name: "FollowUpTypeOption",
                columns: table => new
                {
                    FollowUpTypeOptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpTypeOption", x => x.FollowUpTypeOptionID);
                });

			migrationBuilder.Sql(@"                
                INSERT INTO FollowUpTypeOption(Name)
                VALUES('Commitment- Reporting')

                INSERT INTO FollowUpTypeOption(Name)
                VALUES('Deferral')

                INSERT INTO FollowUpTypeOption(Name)
                VALUES('Employment Verification Form')

                INSERT INTO FollowUpTypeOption(Name)
                VALUES('Job Search Status')

                INSERT INTO FollowUpTypeOption(Name)
                VALUES('Others')

                INSERT INTO FollowUpTypeOption(Name)
                VALUES('Repayment')

                INSERT INTO FollowUpTypeOption(Name)
                VALUES('Waiver Release System')


            ");

			//migrationBuilder.CreateTable(
			//    name: "ProgramYear",
			//    columns: table => new
			//    {
			//        ProgramYearId = table.Column<int>(type: "int", nullable: false)
			//            .Annotation("SqlServer:Identity", "1, 1"),
			//        Name = table.Column<int>(type: "int", nullable: false)
			//    },
			//    constraints: table =>
			//    {
			//        table.PrimaryKey("PK_ProgramYear", x => x.ProgramYearId);
			//    });

			//migrationBuilder.CreateTable(
			//    name: "SessionList",
			//    columns: table => new
			//    {
			//        SessionListId = table.Column<int>(type: "int", nullable: false)
			//            .Annotation("SqlServer:Identity", "1, 1"),
			//        Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
			//    },
			//    constraints: table =>
			//    {
			//        table.PrimaryKey("PK_SessionList", x => x.SessionListId);
			//    });
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "FollowupType");

            migrationBuilder.DropTable(
                name: "FollowUpTypeOption");

            migrationBuilder.DropTable(
                name: "ProgramYear");

            migrationBuilder.DropTable(
                name: "SessionList");
        }
    }
}
