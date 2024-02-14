using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages
{
    public class ReactivateAccountModel : PageModel
    {
        [FromQuery(Name = "ra")]
        public string EncryptedStudentID { get; set; } = "";

        [FromQuery(Name = "at")]
        public string AccountType { get; set; } = "";

        private readonly IMediator _mediator;

        public ReactivateAccountModel(IMediator mediator) => _mediator = mediator;

        public async Task<ActionResult> OnGetAsync()
        {
            await _mediator.Send(new ReactivateAccountCommand() { EncryptedID = EncryptedStudentID, AccountType = AccountType });
            if(AccountType == "ST") return Redirect("/Student/Login");
            if (AccountType == "AO") return Redirect("/Agency/Login");
            if (AccountType == "PI") return Redirect("/Academia/Login");
            if (AccountType == "AD") return Redirect("/Admin/Login");
            return Redirect("/");
        }

        public class ReactivateAccountCommand : IRequest<bool>
        {
            public string EncryptedID { get; set; }
            public string AccountType { get; set; }
        }

        public class ReactivateAccountCommandHandler : IRequestHandler<ReactivateAccountCommand, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICryptoHelper _crypto;
            private readonly ICacheHelper _cache;

            public ReactivateAccountCommandHandler(ScholarshipForServiceContext db, ICryptoHelper crypto, ICacheHelper cache)
            {
                _db = db;
                _crypto = crypto;
                _cache = cache;
            }

            public async Task<bool> Handle(ReactivateAccountCommand request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                var statusOptions = await _cache.GetProfileStatusAsync();
                var activeStatusID = statusOptions.Where(m => m.Name == "Active").Select(m => m.ProfileStatusID).FirstOrDefault();
                int id = Convert.ToInt32(DecryptString(request.EncryptedID, GlobalConfigSettings));
                if (request.AccountType == "ST")
                {
                    var account = await _db.Students.Where(m => m.StudentId == id).FirstOrDefaultAsync();
                    account.ProfileStatusID = activeStatusID;
                    await _db.SaveChangesAsync();
                }
                if (request.AccountType == "PI")
                {
                    var account = await _db.AcademiaUsers.Where(m => m.AcademiaUserId == id).FirstOrDefaultAsync();
                    account.ProfileStatusID = activeStatusID;
                    await _db.SaveChangesAsync();
                }
                if (request.AccountType == "AO")
                {
                    var account = await _db.AgencyUsers.Where(m => m.AgencyUserId == id).FirstOrDefaultAsync();
                    account.ProfileStatusID = activeStatusID;
                    await _db.SaveChangesAsync();
                }
                if (request.AccountType == "AD")
                {
                    var account = await _db.AdminUsers.Where(m => m.AdminUserId == id).FirstOrDefaultAsync();
                    account.IsDisabled = false;
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            private string DecryptString(string input, List<GlobalConfiguration> GlobalConfigSettings)
            {

                EncryptionKeys _keys = new EncryptionKeys();
                _keys.Salt = GlobalConfigSettings.Where(m => m.Key == "Salt" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
                _keys.PassPhrase = GlobalConfigSettings.Where(m => m.Key == "Passphrase" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
                _keys.KeySize = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "Keysize" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
                _keys.InitVector = GlobalConfigSettings.Where(m => m.Key == "InitVect" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
                _keys.PasswordIterations = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "PasswordIterations" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
                return _crypto.Decrypt(input, _keys);
            }
        }

       

    }
}
