using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPM.SFS.Core.Shared;
using OPM.SFS.Web.Mappings;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.SharedCode.Repositories;
using OPM.SFS.Web.SharedCode.StudentDashboardRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Infrastructure.Extensions
{
    public static class StartupExtension
    {
        public static void ConfigureAppServices(this IServiceCollection services, IConfiguration appSettings)
        {
            
            services.AddTransient<ICryptoHelper, CryptoHelper>();
            services.AddTransient<IEmailerService, EmailerService>();
            services.AddTransient<IStudentRegistrationHelper, StudentRegistrationHelper>();
            services.AddTransient<ICacheHelper, CacheHelper>();
            services.AddTransient<INotificationList, NotificationList>();
            services.AddTransient<ICommitmentNotificationService, CommitmentNotificationService>();
            services.AddTransient<ICommitmentProcessService, CommitmentProcessService>();
            services.AddTransient<IStudentProfileValidator, StudentProfileValidator>();
            services.AddTransient<IAntiVirusHelper, AntiVirusHelper>();
            services.AddTransient<IAuditEventLogHelper, AuditEventLogHelper>();
            services.AddTransient<ICommitmentMappingHelper, CommitmentMappingHelper>();
            services.AddTransient<SignInHandler>();
            services.AddTransient<ILoginGovLinkingHelper, LoginGovLinkingHelper>();
            services.AddTransient<IAccountInactiveService, AccountInactiveService>();
            services.AddTransient<IServiceOwedService, ServiceOwedService>();
            services.AddTransient<ITaskHandler, TaskHandler>();
            services.AddTransient<IStudentProfileMappingHelper, StudentProfileMappingHelper>();
            services.AddTransient<IVirusScanner, VirusScanner>();
            services.AddTransient<IStudentDashboardService, StudentDashboardService>();
            services.AddTransient<IAdminDataManagerService, AdminDataManagerService>();
            services.AddTransient<IStudentDashboardLoader, StudentDashboardLoader>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IAcademiaRepository, AcademiaRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
			services.AddTransient<IReferenceDataRepository, ReferenceDataRepository>();
            services.AddTransient<IFeatureManager, FeatureManager>();
            services.AddTransient<ICommonTaskRepository, CommonTaskRepository>();
            services.AddTransient<IStudentDashboardRepository, StudentDashboardRepository>();
			services.AddTransient<IAzureBlobService, AzureBlobService>();
			services.AddTransient<IStudentDashboardRulesEngine, StudentDashboardRulesEngine>();
            services.AddTransient<IUnregisteredReminderEmailService, UnregisteredReminderEmailService>();
            services.AddTransient<IEmploymentVerificationEmailService, EmploymentVerificationEmailService>();
			services.AddTransient<IEmailQueueService, EmailQueueService>();
			services.AddTransient<IUtilitiesService, UtilitiesService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            
        }

        public static void ConfigureValidationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
        }

    }
}
