using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class JobActivityStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
               name: "StatusOther",
               table: "StudentJobActivity",
               type: "varchar(max)",
               nullable: true
              );

            migrationBuilder.Sql(@"update JobActivityStatus set name = 'Applied'
                where code = '1';
                update JobActivityStatus set name = 'Found Qualified'
                where code = '2';
                update JobActivityStatus set name = 'Found Not Qualified'
                where code = '3';
                update JobActivityStatus set name = 'Interview Scheduled'
                where code = '4';
                insert into JobActivityStatus (Name, Code) values ('Interview Completed', '5');
                insert into JobActivityStatus (Name, Code) values ('Offer Received', '6');
                insert into JobActivityStatus (Name, Code) values ('Hired', '7');
                insert into JobActivityStatus (Name, Code) values ('Other', '8');
                ");

            migrationBuilder.Sql(@"declare @other int
                        select @other = JobActivityStatusID from JobActivityStatus where name = 'other'
                        update StudentJobActivity set JobActivityStatusID = @other 
                        where JobActivityStatusID is not null");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "StatusOther",
              table: "StudentJobActivity",
              type: "varchar(max)",
              nullable: false
             );
        }
    }
}
