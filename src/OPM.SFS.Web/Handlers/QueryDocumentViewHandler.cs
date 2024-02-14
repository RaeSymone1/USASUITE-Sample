using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPM.SFS.Web.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.Handlers
{
    public class QueryDocumentView : IRequest<FileStreamResult>
    {
        public int DocumentId { get; set; }
    }

    public class QueryDocumentViewHandler : IRequestHandler<QueryDocumentView, FileStreamResult>
    {
        private readonly IDocumentRepository _documentRepo;
        private readonly IAzureBlobService _blobService;
        public QueryDocumentViewHandler(IDocumentRepository document, IAzureBlobService blobService)
        {
            _documentRepo = document;
            _blobService = blobService;
        }

        public async Task<FileStreamResult> Handle(QueryDocumentView request, CancellationToken cancellationToken)
        {

            var theDocument = await _documentRepo.GetDocumentByDocumentId(request.DocumentId);
            return await _blobService.DownloadDocumentStreamAsync(theDocument.FilePath, theDocument.FileName);
        }
    }
}
