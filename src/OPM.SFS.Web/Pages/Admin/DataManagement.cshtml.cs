using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Models.Admin;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Admin.StudentDashboardModel;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    [IgnoreAntiforgeryToken] //THIS IS JUST FOR TESTING!! NEED TO FIX ASAP!!!!
    public class DataManagementModel : PageModel
    {
		[BindProperty]
		public DataManagementVM Data { get; set; }

		private readonly IMediator _mediator;

		public DataManagementModel(IMediator mediator) => _mediator = mediator;

		public void OnGet()
        {

        }

		public JsonResult OnGetDataOptions(string option)
		{
			var data = _mediator.Send(new JsonQuery() { DataOption = option}).Result;
			return new JsonResult(data);
		}

        public JsonResult OnPostUpdateDataOption([FromBody] DataManagementVM model)
        {
            var data = _mediator.Send(new JsonUpdateDataOptions() { Model = model }).Result;
            return new JsonResult(data);
        }

		public JsonResult OnPostInsertDataOption([FromBody] DataManagementVM model)
		{
			var data = _mediator.Send(new JsonInsertDataOptions() { Model = model }).Result;
			return new JsonResult(data);
		}

		public JsonResult OnPostDeleteOption([FromBody] DataManagementVM model)
        {
            var data = _mediator.Send(new JsonDeleteOptions() { Model = model }).Result;
            return new JsonResult(data);
        }

        public class JsonQuery : IRequest<IEnumerable<DataManagementVM>>
		{
			public string DataOption { get; set; }
		}

		public class JsonQueryHandler : IRequestHandler<JsonQuery, IEnumerable<DataManagementVM>>
		{
			private readonly ScholarshipForServiceContext _efDB;
			private readonly IAdminDataManagerService _service;
            public JsonQueryHandler(ScholarshipForServiceContext efDB, IAdminDataManagerService service)
			{
				_efDB = efDB;
				_service = service;
			}

			public async Task<IEnumerable<DataManagementVM>> Handle(JsonQuery request, CancellationToken cancellationToken)
			{
				if(!string.IsNullOrWhiteSpace(request.DataOption))
				{
					var data = await _service.GetDataOptionsByType(request.DataOption);
					return data;
				}		

				return null;

			}
		}

		public class JsonUpdateDataOptions : IRequest<DataManagementVM>
        {
			public DataManagementVM Model { get; set; }
		}

        public class JsonUpdateStudentHandler : IRequestHandler<JsonUpdateDataOptions, DataManagementVM>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IAdminDataManagerService _service;
            public JsonUpdateStudentHandler(ScholarshipForServiceContext db, IAdminDataManagerService service)
			{
				_efDB = db;
                _service = service;
            }
            public async Task<DataManagementVM> Handle(JsonUpdateDataOptions request, CancellationToken cancellationToken)
            {
                if (!string.IsNullOrWhiteSpace(request.Model.DataGroup))
                {
					_ = await _service.UpdateDataOptions(request.Model.DataGroup, request.Model, "update");
                }
				return new DataManagementVM();
            }
        }

        public class JsonDeleteOptions : IRequest<DataManagementVM>
        {
            public DataManagementVM Model { get; set; }
        }

        public class JsonDeleteOptionHandler : IRequestHandler<JsonDeleteOptions, DataManagementVM>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IAdminDataManagerService _service;
            public JsonDeleteOptionHandler(ScholarshipForServiceContext efDB, IAdminDataManagerService service)
            {
                _efDB = efDB;
                _service = service;
            }

            public async Task<DataManagementVM> Handle(JsonDeleteOptions request, CancellationToken cancellationToken)
            {
                if (!string.IsNullOrWhiteSpace(request.Model.DataGroup))
                {
                    _ = await _service.UpdateDataOptions(request.Model.DataGroup, request.Model, "delete");
                }
				return new DataManagementVM();
            }
        }

        public class JsonInsertDataOptions: IRequest<DataManagementVM>
		{
			public DataManagementVM Model { get; set; }
		}

		public class JsonInsertDataOptionHandler : IRequestHandler<JsonInsertDataOptions, DataManagementVM>
		{
			private readonly ScholarshipForServiceContext _efDB;
            private readonly IAdminDataManagerService _service;

			public JsonInsertDataOptionHandler(ScholarshipForServiceContext db, IAdminDataManagerService service)
			{
				_efDB = db;
				_service = service;
			}
    
			public async Task<DataManagementVM> Handle(JsonInsertDataOptions request, CancellationToken cancellationToken)
			{
                if (!string.IsNullOrWhiteSpace(request.Model.DataGroup))
                {
                    _ = await _service.UpdateDataOptions(request.Model.DataGroup, request.Model, "add");
                }

				return new DataManagementVM() { ID = "0", Name = request.Model.Name, Code = request.Model.Code, DataGroup = request.Model.DataGroup, 
					Option = request.Model.Option, Phase = request.Model.Phase, Placement = request.Model.Placement};
			}
		}
	}

   
}
