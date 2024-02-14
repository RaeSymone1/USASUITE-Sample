using OPM.SFS.Data;
using System.Linq;

namespace OPM.SFS.Web.SharedCode
{
    public interface IStudentProfileValidator
    {
        bool IsBackgroundComplete(int id);
        bool IsProfileComplete(int id);
    }

    public class StudentProfileValidator : IStudentProfileValidator
    {
        private readonly ScholarshipForServiceContext _efDB;

        public StudentProfileValidator(ScholarshipForServiceContext efDB)
        {
            _efDB = efDB;
        }

        public bool IsBackgroundComplete(int id)
        {
            var studentData = _efDB.Students.Where(m => m.StudentId == id)
               .Select(m => new
               {
                   EthnicityID = m.EthnicityId,
                   AddressID = m.CurrentAddressId
               }).FirstOrDefault();

            if (studentData != null)
            {
                if (!studentData.EthnicityID.HasValue || studentData.EthnicityID.Value == 0)
                    return false;
            }
            return true;
        }

        public bool IsProfileComplete(int id)
        {
            var studentData = _efDB.Students.Where(m => m.StudentId == id)
               .Select(m => new
               {
                   EthnicityID = m.EthnicityId,
                   AddressID = m.CurrentAddressId
               }).FirstOrDefault();
            if (studentData != null)
            {

                if (!studentData.AddressID.HasValue || studentData.AddressID.Value == 0)
                    return false;
            }
            return true;
        }
    }
}
