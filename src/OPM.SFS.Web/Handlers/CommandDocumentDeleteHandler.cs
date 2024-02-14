using MediatR;
using OPM.SFS.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OPM.SFS.Web.Handlers
{
    public class CommandDocumentDelete : IRequest<bool>
    {
        public int StudentDocumentId { get; set; }
        public int StudentId { get; set; }
    }

    public class CommandDocumentDeleteHandler : IRequestHandler<CommandDocumentDelete, bool>
    {
        private readonly ScholarshipForServiceContext _efDB;
        public CommandDocumentDeleteHandler(ScholarshipForServiceContext efDB)
        {
            _efDB = efDB;
        }
        public async Task<bool> Handle(CommandDocumentDelete request, CancellationToken cancellationToken)
        {
            var docToDelete = await _efDB.StudentDocuments.Where(m => m.StudentDocumentId == request.StudentDocumentId && m.StudentId == request.StudentId).FirstOrDefaultAsync();
            if (docToDelete is not null)
            {
                docToDelete.IsDeleted = true;
                await _efDB.SaveChangesAsync();
            }

            return true;
        }
    }
}
