using System;

namespace EthansList.Shared
{
    public class FileAccessHelper
    {
        public FileAccessHelper()
        {
        }

        public static string GetLocalFilePath(string filename)
        {
            #if __IOS__
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = System.IO.Path.Combine(docFolder, "..", "Library");

            if (!System.IO.Directory.Exists(libFolder)) {
                System.IO.Directory.CreateDirectory(libFolder);
            }

            return System.IO.Path.Combine(libFolder, filename);
            #elif __ANDROID__
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return System.IO.Path.Combine(path, filename);
            #endif
        }
    }
}

