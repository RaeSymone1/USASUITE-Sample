using System.IO;

namespace OPM.SFS.Web.SharedCode
{

    public class StreamExtensions
    {
        public static byte[] StreamToByteArray(Stream inputStream)
        {
            byte[] lbuffer = new byte[16385];
            using (MemoryStream lmemoryStream = new MemoryStream())
            {
                var lread = inputStream.Read(lbuffer, 0, lbuffer.Length);
                while (lread > 0)
                {
                    lmemoryStream.Write(lbuffer, 0, lread);
                    lread = inputStream.Read(lbuffer, 0, lbuffer.Length);
                }

                return lmemoryStream.ToArray();
            }
        }
    }
}
