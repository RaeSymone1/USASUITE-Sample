using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class StudentLookupModel : PageModel
    {
        [BindProperty]
        public AdminStudentSearchViewModel Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;

        private readonly IMediator _mediator;
        public StudentLookupModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
           
            Data = await _mediator.Send(new Query() { StudentID = StudentID});
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                return Page();
            }
            Data = await _mediator.Send(new Query( ) { SearchFilter = Data.SearchFilter, SearchOption = Data.SearchOption});
            return Page();
        }

        public class AdminStudentSearchViewModelValidator : AbstractValidator<AdminStudentSearchViewModel>
        {
            public AdminStudentSearchViewModelValidator()
            {
                RuleFor(x => x.SearchFilter).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.SearchOption).NotEmpty();
            }
        }

      

       public class Query : IRequest<AdminStudentSearchViewModel>
        {
            public string SearchOption { get; set; }
            public string SearchFilter { get; set; }
            public int StudentID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminStudentSearchViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICryptoHelper _crypto;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICryptoHelper crypto, ICacheHelper cache)
            {
                _db = db;
                _crypto = crypto;
                _cache = cache;
            }          
            
            public async Task<AdminStudentSearchViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                AdminStudentSearchViewModel model = new();
                model.Students = new();
                if(request.StudentID > 0)
                {
                    var studentByID = await _db.Students
                       .Where(m => m.StudentId == request.StudentID)
                       .Select(m => new
                       {
                           StudentID = m.StudentId,
                           UID = m.StudentUID,
                           Lastname = m.LastName,
                           Firstname = m.FirstName,
                           Email = m.Email,
                           SSN = m.Ssn,
                           Gender = m.Gender,
                           Status = m.ProfileStatus.Name
                       })
                       .ToListAsync();

                    foreach (var item in studentByID)
                    {
                        var ssn = "000000000";
                        if (!string.IsNullOrWhiteSpace(item.SSN))
                        {
                           ssn =  _crypto.Decrypt(item.SSN, GlobalConfigSettings);
                        }
                        bool isBackgroundComplete = item.Gender != null && item.Gender.GenderId > 0 ? true : false;
                        bool profileComplete = true;
                        model.Students.Add(new AdminStudentSearchViewModel.StudentRecords()
                        {
                            StudentID = item.StudentID,
                            StudentUID = item.UID,
                            LastName = item.Lastname,
                            FirstName = item.Firstname,
                            Email = item.Email,
                            BackgroundComplete = isBackgroundComplete ? "Yes" : "No",
                            ProfileComplete = profileComplete ? "Yes" : "No",
                            SSN = ssn.Substring(ssn.Length - 4),
                            Status = item.Status
                        });
                    }
                    return model;
                }

                if (string.IsNullOrWhiteSpace(request.SearchOption) && string.IsNullOrWhiteSpace(request.SearchFilter))
                {
                    model.SearchOption = "Lastname";
                    model.SearchResultsMessage = "";
                    return model;
                }

                var result = await _db.Students.AsNoTracking()
                        .WhereIf(request.SearchOption.Equals("lastname", System.StringComparison.CurrentCultureIgnoreCase), m => m.LastName == request.SearchFilter)
                        .WhereIf(request.SearchOption.Equals("email", System.StringComparison.CurrentCultureIgnoreCase), m => m.Email == request.SearchFilter)
                        .Select(m => new
                        {
                            StudentID = m.StudentId,
                            UID = m.StudentUID,
                            Lastname = m.LastName,
                            Firstname = m.FirstName,
                            Email = m.Email,
                            SSN = m.Ssn,
                            Gender = m.Gender,
                            Status = m.ProfileStatus.Name,
                            Display = m.ProfileStatus.Display 
                        })
                        .ToListAsync();
                foreach (var item in result)
                {
                    var ssn = "000000000";
                    if (!string.IsNullOrWhiteSpace(item.SSN))
                    {
                        ssn = _crypto.Decrypt(item.SSN, GlobalConfigSettings);
                    }
                    bool isBackgroundComplete = item.Gender != null && item.Gender.GenderId  > 0 ? true : false;
                    bool profileComplete = true;
                    model.Students.Add(new AdminStudentSearchViewModel.StudentRecords()
                    {
                        StudentID = item.StudentID,
                        StudentUID = item.UID,
                        LastName = item.Lastname,
                        FirstName = item.Firstname,
                        Email = item.Email,
                        BackgroundComplete = isBackgroundComplete ? "Yes" : "No",
                        ProfileComplete = profileComplete ? "Yes" : "No",
                        SSN = ssn.Substring(ssn.Length - 4),
                        Status = item.Status,
                        Display = item.Display
                    });
                }

                if (model.Students.Count == 0)
                    model.SearchResultsMessage = "No students found.";

                return model;
            }
        }

        
    }
}
