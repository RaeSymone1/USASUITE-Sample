using OPM.SFS.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.Repositories
{
    public interface ICommonTaskRepository
    {
        Task InsertEmailLog(string body, string to, DateTime senddate, string subject);
        Task<EmailTemplate> GetEmailTemplateByCode(string code);
    }
    public class CommonTaskRepository : ICommonTaskRepository
    {
        private readonly ScholarshipForServiceContext _efDB;

        public CommonTaskRepository(ScholarshipForServiceContext efDB)
        {
            _efDB = efDB;
        }

        public async Task InsertEmailLog(string body, string to, DateTime senddate, string subject)
        {
            _efDB.EmailSentLog.Add(new EmailSentLog() { Body = body, Recipients = to, SentDate = senddate, Subject = subject });
            await _efDB.SaveChangesAsync();
        }

        public async Task<EmailTemplate> GetEmailTemplateByCode(string code)
        {
            return _efDB.EmailTemplates.Where(m => m.Code == code).FirstOrDefault();
        }
    }
}
