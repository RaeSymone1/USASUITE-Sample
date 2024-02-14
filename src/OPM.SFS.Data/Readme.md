#Under construction!!
#Database development guide using EF Core code first - https://entityframeworkcore.com/approach-code-first
1. Schema changes: 2 options
   - Option 1: Updating the entity classes and runnng migrations
   - Option 2: Updating your local DB and running a reverse engineerng scaffold to update the entites
2. Create migration for all database changes. Review migration and fix any data types. Manually add any data changes to your migration file. 


#Helpful commands for terminal
- Reverse engineer from existing database - dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ScholarshipForService" Microsoft.EntityFrameworkCore.SqlServer --output-dir Data --project OPM.SFS.Core
- Adding migration for DB changes - dotnet ef migrations add MigrationName --project OPM.SFS.Core.csproj --output-dir Migrations
- Run migration to update LocalDB - dotnet ef database update --project OPM.SFS.Core.csproj --connection "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ScholarshipForService"

#Using Stored Procedures!
- Dapper is available for stored procedures. Recommended usage for complex queries requiring multiple joins
- To use dapper: 
	var sql = "exec [GetStudentByUserName] @UserName";
	var values = new { UserName = request.UserId };
	var spResults = await _db.QueryAsyncWithDapper<Data>(cancellationToken, sql, values);


#Potential Errors -
The Entity Framework tools version x.x.x is older than that of the runtime = upgrade the  CLI tools with the following commands - dotnet tool update --global dotnet-ef
