using OPM.SFS.Data;
using System.Linq;
using System.Threading.Tasks;
namespace OPM.SFS.Web.SharedCode
{
    public interface ITaskHandler
    {
        Task<bool> RunTaskAsync(string code, string type, string batch);
    }

    public class TaskHandler : ITaskHandler
    {
        private readonly IAccountInactiveService _accountService;
        private readonly IStudentDashboardLoader _dashboardLoader;
        private readonly IUnregisteredReminderEmailService _unregisteredReminderEmailService;
        private readonly IEmploymentVerificationEmailService _evfEmailer;

        public TaskHandler(IAccountInactiveService accountService, IStudentDashboardLoader dashboardLoader, IUnregisteredReminderEmailService unregisteredReminderEmailService, IEmploymentVerificationEmailService evfEmailer)
        {
            _accountService = accountService;
            _dashboardLoader = dashboardLoader;
            _unregisteredReminderEmailService = unregisteredReminderEmailService;
            _evfEmailer = evfEmailer;
        }

        public async Task<bool> RunTaskAsync(string code, string type, string batch)
        {
            if (code == "inactiveStudents")
            {
                await _accountService.SetInactiveStudentsAsync();
            }

            if (code == "inactivePIs")
            {
                await _accountService.SetInactiveAcademiaUsers();
            }

            if (code == "inactiveAOs")
            {
                await _accountService.SetInactiveAgencyUsers();
            }

            if (code == "inactiveAdmins")
            {
                await _accountService.SetInactiveAdmins();
            }

            if (code == "reminderInactiveAccounts")
            {
                await _accountService.SendReminderEmailAsync(type);
            }

            if (code == "UnregisteredReminder")
            {
                await _unregisteredReminderEmailService.SendReminderEmailAsync();
            }
            if (code == "EVFDueReminder")
                await _evfEmailer.SendAllEVFEmails();

            return true;
        }
    }
}
