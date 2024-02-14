using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Data.Migrations;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Models.Academia;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]
    public class StudentProfileModel : PageModel
    {
        [BindProperty]
        public AcademiaStudentProfileViewModel Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;

        private readonly IMediator _mediator;

        public StudentProfileModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { Id = StudentID});
        }

        public async Task<FileResult> OnGetViewDocument(int id)
        {
            var file = await _mediator.Send(new QueryDocumentView() { DocumentId = id });
            return file;
        }

        public class Query : IRequest<AcademiaStudentProfileViewModel>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AcademiaStudentProfileViewModel>
        {
            private readonly IAcademiaRepository _academiaRepository;


            public QueryHandler(IAcademiaRepository academiaRepository)
            {
                _academiaRepository = academiaRepository;

            }

            public async Task<AcademiaStudentProfileViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _academiaRepository.GetStudentProfile(request.Id);
            }



        }

        public class QueryDocumentView : IRequest<FileStreamResult>
        {
            public int DocumentId { get; set; }
        }

        public class QueryDocumentViewHandler : IRequestHandler<QueryDocumentView, FileStreamResult>
        {
            private readonly IDocumentRepository _documentRepo;
            private readonly IAzureBlobService _blobService;
            private readonly IConfiguration _appSettings;

            public QueryDocumentViewHandler(IDocumentRepository document,IAzureBlobService blobService, IConfiguration appSettings)
            {
                _documentRepo = document;
                _blobService = blobService;
                _appSettings = appSettings;
            }

            public async Task<FileStreamResult> Handle(QueryDocumentView request, CancellationToken cancellationToken)
            {
                if(_appSettings["General:Hosting"] == "Macon")
                {
                    return await _documentRepo.GetDocumentStreamByDocumentId(request.DocumentId);
                }
                else
                {
                    var theDocument = await _documentRepo.GetDocumentByDocumentId(request.DocumentId);
                    return await _blobService.DownloadDocumentStreamAsync(theDocument.FilePath, theDocument.FileName);
                }
               
            }
        }
    }
}
