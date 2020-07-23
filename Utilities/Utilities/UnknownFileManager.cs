using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ista.Utilities
{
    /// <summary>
    /// This class finds for any .ff "unknonw" file type the real .ff file.
    /// </summary>
    public class UnknownFileManager
    {
        public List<string> FileNames { get; private set;}
        public UnknownFileManager(string fileName)
        {
            string sourceFile = Path.GetFileName(fileName);
            string filePath = Path.GetDirectoryName(fileName);
            string[] nameDivided = sourceFile.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            FileNames = new List<string>();
            if (nameDivided.Contains("Unknown.ff"))
            {
                string mainFile = nameDivided[0] + "_" + nameDivided[1] + ".ff";
                string fullmain = Path.Combine(filePath, mainFile);

                if (File.Exists(fullmain))
                    FileNames.Add(fullmain);
                FileNames.Add(fileName);
            }
            else
            {
                FileNames.Add(fileName);
                string unknown = sourceFile.Remove(sourceFile.Length - 3, 3) + "_Unknown.ff";
                string fullmain = Path.Combine(filePath, unknown);

                if(File.Exists(fullmain))
                    FileNames.Add(fullmain);
            }
        }
    }
}
