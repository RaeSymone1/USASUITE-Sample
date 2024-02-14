using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public interface IFeatureManager
    {
        Task<bool> IsEnabledSiteWideAsync(string feature);
        Task<bool> IsEnabledForAcademiaUserAsync(string feature, int academiaUserId);
        Task<bool> IsEnabledForAdminAsync(string feature, int adminId);
        Task<bool> IsEnabledForAgencyUserAsync(string feature, int agencyUserId);
        Task<bool> IsEnabledForStudentAsync(string feature, int studentId);
    }

    public class FeatureManager : IFeatureManager
    {
        private readonly ScholarshipForServiceContext _db;
        public FeatureManager(ScholarshipForServiceContext db) => _db = db;

        public async Task<bool> IsEnabledSiteWideAsync(string feature)
        {
            var isEnabled = await _db.Feature.Where(m => m.Name == feature).Select(m => m.IsEnabledSiteWide).FirstOrDefaultAsync();
            return isEnabled;
        }

        public async Task<bool> IsEnabledForStudentAsync(string feature, int studentId)
        {
            var featureUsers = await _db.StudentFeature
                .Where(m => m.Feature.Name == feature)
                .Where(m => m.IsEnabled)
                .Where(m => m.StudentId == studentId)
                .Select(m => m.StudentId).ToListAsync();

            if (featureUsers.Any()) return true;
            return false;
        }

        public async Task<bool> IsEnabledForAdminAsync(string feature, int adminId)
        {
            var featureUsers = await _db.AdminFeature
                .Where(m => m.Feature.Name == feature)
                .Where(m => m.IsEnabled)
                .Where(m => m.AdminUserId == adminId)
                .Select(m => m.AdminUserId).ToListAsync();

            if (featureUsers.Any()) return true;
            return false;
        }

        public async Task<bool> IsEnabledForAcademiaUserAsync(string feature, int academiaUserId)
        {
            var featureUsers = await _db.AcademiaUserFeature
                .Where(m => m.Feature.Name == feature)
                .Where(m => m.IsEnabled)
                .Where(m => m.AcademiaUserId == academiaUserId)
                .Select(m => m.AcademiaUserId).ToListAsync();

            if (featureUsers.Any()) return true;
            return false;
        }

        public async Task<bool> IsEnabledForAgencyUserAsync(string feature, int agencyUserId)
        {
            var featureUsers = await _db.AgencyUserFeature
                .Where(m => m.Feature.Name == feature)
                .Where(m => m.IsEnabled)
                .Where(m => m.AgencyUserId == agencyUserId)
                .Select(m => m.AgencyUserId).ToListAsync();

            if (featureUsers.Any()) return true;
            return false;
        }
    }
}
