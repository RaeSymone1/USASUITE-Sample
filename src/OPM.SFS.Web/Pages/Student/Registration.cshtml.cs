using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using static OPM.SFS.Web.Shared.CustomAnnotations;

namespace OPM.SFS.Web.Pages.Student
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class RegistrationModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public RegistrationCodeRequest Data { get; set; }

        public RegistrationModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var response = await _mediator.Send(Data);
            if (response.IsSuccessful) return RedirectToPage("/Student/RegistrationDetails", new { uid = Data.Code });
            ModelState.AddModelError("Code", response.ErrorMessage);
            return Page();
        }
    }

    public class RegistrationCodeRequest : IRequest<RegistrationCodeResponse>
    {       
        public string Code { get; set; }

    }

    public class RegistrationCodeResponse : IRequest
    {        
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }

    //Validations
    public class RegistrationValidator : AbstractValidator<RegistrationCodeRequest>
    {
        public RegistrationValidator()
        {
            RuleFor(request => request.Code).NotEmpty();
            RuleFor(request => request.Code).Matches("^[^><&]+$");
        }
    }

    public class RegistrationCodeHandler : IRequestHandler<RegistrationCodeRequest, RegistrationCodeResponse>
    {
        
        private readonly ICryptoHelper _crypto;
		private readonly IReferenceDataRepository _refRepo;
        private readonly IStudentRepository _repo;
		private readonly ILogger<RegistrationModel> _logger;
        private readonly IFeatureManager _featureManager;
        private readonly IStudentRegistrationHelper _regHelper;

        public RegistrationCodeHandler(ICryptoHelper crypto, IReferenceDataRepository refRepo, IStudentRepository repo, ILogger<RegistrationModel> logger, IFeatureManager featureManager, IStudentRegistrationHelper helper)
        {
            _crypto = crypto;
            _refRepo = refRepo;
            _repo = repo;
            _logger = logger;
            _featureManager = featureManager;
            _regHelper = helper;
        }

        public async Task<RegistrationCodeResponse> Handle(RegistrationCodeRequest request, CancellationToken cancellationToken)
        {
            
			var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
			try
			{
                if (await _featureManager.IsEnabledSiteWideAsync("UnregisteredStudentFlow"))
                {
                    var decrypt = _crypto.Decrypt(request.Code, GlobalConfigSettings);
                    if (!string.IsNullOrWhiteSpace(decrypt) && int.TryParse(decrypt, out int studentID))
                    {
                        var studentLookup = await _repo.GetStudentByStudentUID(studentID);                       
                        if(studentLookup == null) return new RegistrationCodeResponse() { ErrorMessage = "Please enter a valid registration code", IsSuccessful = false };
                        if (studentLookup.ProfileStatus.Name != "Not Registered")
                        {
                            return new RegistrationCodeResponse() { ErrorMessage = "Please enter a valid registration code", IsSuccessful = false };
                        }                       
                       return new RegistrationCodeResponse() { ErrorMessage = "", IsSuccessful = true };
                    }
                }
                else
                {
                    var lookupCode = await _repo.GetStudentRegistrationCode(request.Code);
                    bool isValid = _regHelper.ValidateCode(lookupCode);
                    if (isValid)
                        return new RegistrationCodeResponse() { ErrorMessage = "", IsSuccessful = true };
                    return new RegistrationCodeResponse() { ErrorMessage = "Please enter a valid registration code", IsSuccessful = false };
                }
                    
			}
            catch (Exception ex) 
            {
                _logger.LogError($"Registration error for code: {request.Code}");
				_logger.LogError($"Error message {ex.Message}. Stack {ex.StackTrace}");

			}
			return new RegistrationCodeResponse() { ErrorMessage = "Please enter a valid registration code", IsSuccessful = false };
		}       

    }
}
