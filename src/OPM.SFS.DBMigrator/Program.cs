using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();

app.MapGet("/", () => "SFS Database Migrator - Run EF Core Migrations!");


string connString = builder.Configuration.GetConnectionString("DBConnection");

var optionsBuilder = new DbContextOptionsBuilder<ScholarshipForServiceContext>();
optionsBuilder.UseSqlServer(connString);
using (var context = new ScholarshipForServiceContext(optionsBuilder.Options))
{
	List<string> migrationsToApply = context.Database.GetPendingMigrations().ToList();
	if (migrationsToApply.Count > 0)
	{
		await context.Database.MigrateAsync();

	}

}


app.Run();



