using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Shared
{
    public interface INotificationList
    {
        Task<string> PI_NotificationListAsync(int instititionID);
        string PO_NotificationList();
    }

    public class NotificationList : INotificationList
    {
        private readonly ScholarshipForServiceContext _db;
        private readonly ICacheHelper _cache;

        public NotificationList(ScholarshipForServiceContext db, ICacheHelper cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<string> PI_NotificationListAsync(int instititionID)
        {
            var institutions = await _cache.GetInstitutionsAsync();
            var parentInstitution = institutions.Where(m => m.InstitutionId== instititionID).Select(m => m.ParentInstitutionID).FirstOrDefault();
            if (parentInstitution != null)
            {
                var piList = await _db.AcademiaUsers
                            .Where(m => m.InstitutionID == parentInstitution)
                            .Where(m => m.ProfileStatus.Name != "Disabled")
                            .Select(m => new
                            {
                                m.Email
                            }).ToListAsync();

                string delimitedToList = String.Join(";", piList.Select(m => m.Email).ToArray());
                return delimitedToList;
            }
            else
            {
                var piList = await _db.AcademiaUsers
                            .Where(m => m.InstitutionID == instititionID)
                            .Where(m => m.ProfileStatus.Name != "Disabled")
                            .Select(m => new
                            {
                                m.Email
                            }).ToListAsync();

                string delimitedToList = String.Join(";", piList.Select(m => m.Email).ToArray());
                return delimitedToList;
            }
        }

        public string PO_NotificationList()
        {
            return "sfs@opm.gov";
        }
    }
}
