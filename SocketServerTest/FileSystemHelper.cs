using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerTest
{
    public static class FileSystemHelper
    {
        #region ----------------------------------- Public methods -------------------------------------------------

        public static bool FileExists(string currentDir, string filePath, Action<string> logger)
        {
            bool toReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(currentDir) && !string.IsNullOrEmpty(filePath))
                {
                    string fullFilePath = currentDir + filePath;

                    string[] files = System.IO.Directory.GetFiles(currentDir);
                    if (files.Where(file => file.ToLower().Contains(filePath.ToLower())).ToList().Count > 0)
                    {
                        toReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger(ex.Message);
            }

            return toReturn;
        }

        public static byte[] FileAsByteArray(string fullFilePath)
        {
            return File.ReadAllBytes(fullFilePath);
        }

        public static byte[] FileAsByteArray(string directoryPath, string filePath)
        {
            return (FileAsByteArray(directoryPath + filePath));
        }

        #endregion
    }
}
