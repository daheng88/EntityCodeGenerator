using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Microsoft.DbContextPackage.Utilities
{
    internal class FileGenerator
    {

        public static void AddNewFile(string path, string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
           
            File.WriteAllText(path, contents);

        
        }
    }
}
