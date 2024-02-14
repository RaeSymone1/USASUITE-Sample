using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class RegistrationCodesModel : PageModel
    {
        [BindProperty]
        public AdminRegistrationCodesViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public RegistrationCodesModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public class Query : IRequest<AdminRegistrationCodesViewModel>
        {

        }

        public class QueryHandler : IRequestHandler<Query, AdminRegistrationCodesViewModel>
        {

            private readonly ScholarshipForServiceContext _db;
            public QueryHandler(ScholarshipForServiceContext db)
            {
                _db = db;
            }

            public async Task<AdminRegistrationCodesViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AdminRegistrationCodesViewModel model = new();
                model.RegistrationCodes = new();

                var codes = await _db.RegistrationCodes.AsNoTracking().ToListAsync();
                DateTime CodeExpirationDate = DateTime.Now;
                CodeExpirationDate = CodeExpirationDate.AddMonths(-3);
                foreach (var item in codes)
                {
                    if (item.QuarterStartDate > CodeExpirationDate)
                    {
                        model.RegistrationCodes.Add(new AdminRegistrationCodesViewModel.RegistrationCodeItem()
                        {
                            Code = item.Code,
                            Quartername = item.QuarterName
                        });
                    }
                }
				model.RegistrationCodes = model.RegistrationCodes.OrderBy(x => x.Quartername).ToList();
                return model;
            }
        }


    }
}
