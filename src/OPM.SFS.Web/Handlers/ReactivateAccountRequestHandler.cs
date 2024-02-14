using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Handlers
{
    public class ReactivateAccountRequest : IRequest<ReactivateAccountVM>
    {
        public string EncryptedID { get; set; }
        public string AccountType { get; set; }
    }

    public class ReactivateAccountVM
    {
        public bool IsAccountInactive { get; set; }
        public string EncryptedStudentID { get; set; }
        public bool ShowSuccessEmail { get; set; }
        public string ReactivateUrl { get; set; }
    }

    public class InactiveAccount
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
    }

    public class ReactivateAccountRequestHandler : IRequestHandler<ReactivateAccountRequest, ReactivateAccountVM>
    {
        private readonly ScholarshipForServiceContext _efDB;
        private readonly IEmailerService _emailer;
        private readonly ICryptoHelper _crypto;
        private readonly ICacheHelper _cache;
        private readonly IConfiguration _appSettings;


        public ReactivateAccountRequestHandler(ScholarshipForServiceContext db, IEmailerService emailer, ICryptoHelper crypto, ICacheHelper cache, IConfiguration appSettings)
        {
            _efDB = db;
            _emailer = emailer;
            _crypto = crypto;
            _cache = cache;
            _appSettings = appSettings;
        }

        public async Task<ReactivateAccountVM> Handle(ReactivateAccountRequest request, CancellationToken cancellationToken)
        {
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            int id = Convert.ToInt32(Decrypt(request.EncryptedID, GlobalConfigSettings));
            string baseUrl = _appSettings["General:BaseUrl"];
            var data = await GetAccountAsync(request.AccountType, id);
            var secureLink = $"{baseUrl}/ReactivateAccount?ra={Uri.EscapeDataString(request.EncryptedID)}&at={request.AccountType}";
            string emailContent = $@"Hello, {data.FirstName} <br/><br/>
                                    We received your request to reactivate your SFS account. Please use the link below to sign in to your SFS account to reactiate your account.
                                    This link will expire in 24 hours <br/><br/>
                                    {secureLink}";
            await _emailer.SendEmailDefaultTemplateAsync(data.Email, "SFS - Reactivate your account", emailContent);
            return new ReactivateAccountVM() { EncryptedStudentID = request.EncryptedID, ShowSuccessEmail = true };
        }

        private string Decrypt(string input, List<GlobalConfiguration> GlobalConfigSettings)
        {
            EncryptionKeys _keys = new EncryptionKeys();
            _keys.Salt = GlobalConfigSettings.Where(m => m.Key == "Salt" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PassPhrase = GlobalConfigSettings.Where(m => m.Key == "Passphrase" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.KeySize = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "Keysize" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            _keys.InitVector = GlobalConfigSettings.Where(m => m.Key == "InitVect" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PasswordIterations = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "PasswordIterations" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            return _crypto.Decrypt(input.Trim(), _keys);
        }

        private async Task<InactiveAccount> GetAccountAsync(string accountType, int id)
        {
            InactiveAccount _account = null;
            if(accountType == "ST")
            {
                _account = await _efDB.Students.Where(m => m.StudentId == id).Select(m => new InactiveAccount()
                {
                    Email = m.Email,
                    FirstName = m.FirstName
                }).FirstOrDefaultAsync();
                
            }

            if(accountType == "AO")
            {
                _account = await _efDB.AgencyUsers.Where(m => m.AgencyUserId == id).Select(m => new InactiveAccount()
                {
                    Email = m.Email,
                    FirstName = m.Firstname
                }).FirstOrDefaultAsync();
            }

            if(accountType == "PI")
            {
                _account = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == id).Select(m => new InactiveAccount()
                {
                    Email = m.Email,
                    FirstName = m.Firstname
                }).FirstOrDefaultAsync();
            }

            if(accountType == "AD")
            {
                _account = await _efDB.AdminUsers.Where(m => m.AdminUserId == id).Select(m => new InactiveAccount()
                {
                    Email = m.Email,
                    FirstName = m.FirstName
                }).FirstOrDefaultAsync();
            }

            return _account;
        }
    }
}
