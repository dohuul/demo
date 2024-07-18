using Amazon.Runtime.Internal.Util;
using NPOI.Util;
using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    public class StreamUtilities
    {
        public static void CopyStream(Stream copyFrom, Stream copyTo)
        {
            if (copyFrom == null) throw new ArgumentNullException("null copyFrom in CopyStream at QA.Common.Utilities.StreamUtilities");
            if (copyTo == null) throw new ArgumentNullException("null copyTo in CopyStream at QA.Common.Utilities.StreamUtilities");

            if (copyFrom.CanSeek) copyFrom.Seek(0, SeekOrigin.Begin);
            if (copyTo.CanSeek) copyTo.Seek(0, SeekOrigin.Begin);

            int bytesLength = 65536; //keep below 85K to stay away from large object heap
            byte[] buffer = new byte[bytesLength];

            int bytesRead = copyFrom.Read(buffer, 0, bytesLength);

            while(bytesRead > 0)
            {
                copyTo.Write(buffer, 0, bytesRead);
                bytesRead = copyFrom.Read(buffer, 0, bytesLength);
            }               
        }

        public static string StreamToTempFile(Stream stream, string fileExtension)
        {
            string tempFile = Path.GetTempFileName();
            using (FileStream fs = File.Open(tempFile, FileMode.Open, FileAccess.Write))
            {
                stream.CopyTo(fs);
                fs.Flush();
            }

            if(fileExtension != null && fileExtension.Length > 0)
            {
                string fileWithExtension = Path.Combine(
                        Path.Combine(Path.GetDirectoryName(tempFile),
                        Path.GetFileNameWithoutExtension(tempFile) + "." + fileExtension));

                if (File.Exists(fileWithExtension))
                {
                    File.Delete(fileWithExtension);
                    File.Move(tempFile, fileWithExtension);
                    return fileWithExtension;                }
            }
            return tempFile;
        }

        public static byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read; 
                while((read = input.Read(buffer,0,buffer.Length))> 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
