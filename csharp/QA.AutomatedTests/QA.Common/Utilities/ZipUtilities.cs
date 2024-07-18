using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    public class ZipUtilities
    {
        public static void CopyTo(Stream sourceStream, Stream destinationStream)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while((cnt = sourceStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                destinationStream.Write(bytes, 0, cnt);
            }
        }

        public static byte[] GZIPCompress(string stringValue)
        {
            var bytes = Encoding.UTF8.GetBytes(stringValue);
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        CopyTo(inputStream, gzipStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

        public static String GZIPDecompress(byte[] bytes)
        {
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        CopyTo(gzipStream, outputStream);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }
    }
}
