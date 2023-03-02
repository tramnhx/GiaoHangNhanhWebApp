using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace BHSNetCoreLib
{
    public class FileUtil
    {
        public static byte[] Download(string filename)
        {
            return System.IO.File.ReadAllBytes(filename);
        }

        public static string Unzip(string sourceFile, string destinationFolderName)
        {
            string resultMessage = string.Empty;
            try
            {
                ZipFile.ExtractToDirectory(sourceFile, destinationFolderName);
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }
        public static List<ZipArchiveEntry> GetContent(string sourceFile)
        {
            return ZipFile.OpenRead(sourceFile).Entries.ToList();
        }

    }
}
