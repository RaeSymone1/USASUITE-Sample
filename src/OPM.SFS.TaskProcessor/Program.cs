using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.TaskProcessor;
using OPM.SFS.TaskProcessor.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<ScholarshipForServiceContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
});

builder.Services.AddTransient<IAzureEmailClient, AzureEmailClient>();
builder.Services.AddTransient<IAzureBlobService, AzureBlobService>();
builder.Services.AddTransient<IEmailerService, EmailerService>();
builder.Services.AddTransient<IEmailQueueService, EmailQueueService>();
builder.Services.AddTransient<ICryptoHelper, CryptoHelper>();
builder.Services.AddTransient<IUtilitiesService, UtilitiesService>();
builder.Services.AddHostedService<SendEmailTask>();
builder.Services.AddHostedService<InactiveAccountReminderTask>();
builder.Services.AddHostedService<SetAccountInactiveTask>();
builder.Services.AddHostedService<StudentRegistrationReminderTask>();
builder.Services.AddHostedService<CheckMalwareResultsTask>();
builder.Services.AddHostedService<ServiceObligationCompleteReminderTask>();
builder.Services.AddHostedService<PostgradVerificationDueReminderTask>();

var app = builder.Build();

app.MapGet("/", () => "SFS Task Processor is running!");

app.Run();
