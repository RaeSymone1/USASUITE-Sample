using Microsoft.AspNetCore.Http;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public interface IAuditEventLogHelper
    {
        Task<bool> LogAuditEvent(string message, string additionalInfo = "");
    }

    public class AuditEventLogHelper : IAuditEventLogHelper
    {
        private readonly ScholarshipForServiceContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditEventLogHelper(ScholarshipForServiceContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LogAuditEvent(string message, string additionalInfo)
        {
            int userClaim = _httpContextAccessor.HttpContext.User.GetUserId();
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var userID = userClaim;
            var role = userRole != null ? userRole.Value : "";
            var e = new AuditEvent() { UserID = userID.ToString(), IPAddress = ip, Role = role, AdditionalInfo = additionalInfo };

            _db.AuditEventLog.Add(new AuditEventLog()
            {
                Message = message,
                Timestamp = DateTime.UtcNow,
                LogEvent = JsonSerializer.Serialize(e)
            });
            await _db.SaveChangesAsync();
            return true;
        }
    }

    public class AuditEvent
    {
        public string UserID { get; set; }
        public string Role { get; set; }
        public string IPAddress { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
