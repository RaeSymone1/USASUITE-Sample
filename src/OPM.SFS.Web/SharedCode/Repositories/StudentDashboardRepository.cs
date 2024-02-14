using OPM.SFS.Core.DTO;
using OPM.SFS.Data;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.Repositories
{
    public interface IStudentDashboardRepository
    {
        Task InsertStudentDashboardLog(StudentDashboardLogDTO before, StudentDashboardLogDTO after, int adminID);
    }
    public class StudentDashboardRepository : IStudentDashboardRepository
    {
        private readonly ScholarshipForServiceContext _efDB;

        public StudentDashboardRepository(ScholarshipForServiceContext efDB)
        {
            _efDB = efDB;
        }

        public async Task InsertStudentDashboardLog(StudentDashboardLogDTO before, StudentDashboardLogDTO after, int adminID)
        {
            string beforejson = JsonSerializer.Serialize(before);
            string afterjson = JsonSerializer.Serialize(after);
            _efDB.StudentDashboardLog.Add(new Core.Data.StudentDashboardLog() { AdminID = adminID, BeforeChange = beforejson, AfterChange = afterjson, TimeStamp = DateTime.UtcNow });
            await _efDB.SaveChangesAsync();
        }
    }
}
