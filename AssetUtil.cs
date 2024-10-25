using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class AssetUtil
    {
        public static void LoadAssets(string folder, List<string> output)
        {
            var directory = DirAccess.Open(folder);
            directory.IncludeHidden = true;
            string[] files = directory.GetFiles();
            foreach (string file in files)
            {
                string path = Path.Combine(folder, file);
                if (path.Contains(".import"))
                    output.Add(path.Replace(".import", ""));
                
            }
        }
    }
}
