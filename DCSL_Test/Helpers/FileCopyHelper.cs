using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSL_Test.Helpers
{
    public static class FileCopyHelper
    {
        public static void CopyDirectoryContentRecursive(string sourcePath, string destinationPath, Action<string> writerFunc)
        {
            try
            {
                var sourceDirectories = System.IO.Directory.GetDirectories(sourcePath);
                foreach (var sourceDirectory in sourceDirectories)
                {
                    var subDirPath = sourceDirectory.Remove(0, sourcePath.Length);
                    var fullDestDirPath = destinationPath + subDirPath;
                    if (!System.IO.Directory.Exists(fullDestDirPath))
                    {
                        System.IO.Directory.CreateDirectory(fullDestDirPath);
                    }

                    CopyFiles(sourceDirectory, fullDestDirPath, writerFunc);

                    CopyDirectoryContentRecursive(sourceDirectory, fullDestDirPath, writerFunc);
                }
            }
            catch (Exception ex)
            {
                writerFunc("Error occured whilst trying to copy: " + ex.Message);
            }
        }

        public static void CopyFiles(string sourcePath, string destPath, Action<string> writerFunc)
        {
            var sourceFiles = System.IO.Directory.GetFiles(sourcePath);
            foreach (var sourceFile in sourceFiles)
            {
                try
                {
                    var filePath = sourceFile.Remove(0, sourcePath.Length);
                    System.IO.File.Copy(sourceFile, destPath + filePath);
                    writerFunc(string.Format("{0} copyed to: {1}", sourceFile, destPath + filePath));
                }
                catch (Exception ex)
                {
                    writerFunc(string.Format("{0} copyed to: {1}, Message: {2}", sourceFile, destPath, ex.Message));
                }
            }
        }
    }
}
