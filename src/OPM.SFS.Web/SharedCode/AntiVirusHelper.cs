using OPM.SFS.Web.Shared;
using System.IO;

namespace OPM.SFS.Web.SharedCode
{
    public interface IAntiVirusHelper
    {
        bool IsDocAVClean(Stream doc, string server, string userID);
    }

    public class AntiVirusHelper : IAntiVirusHelper
    {
        private readonly IVirusScanner _scanner;
        public AntiVirusHelper(IVirusScanner scanner)
        {
            _scanner = scanner;
        }
        public bool IsDocAVClean(Stream doc, string server, string userID)
        {
            _scanner.InitalizeVirusScanner(server, userID);
            var avResult = _scanner.ScanForVirus(doc, userID);
            if (avResult.IsResultClean)
                return true;
            return false;

        }
    }
}
