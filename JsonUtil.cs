using Godot;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public partial class JsonUtil
    {
        /// <summary>
        /// Save an object as JSON.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="saveObject"></param>
        /// <param name="path"></param>
        public static void SaveObject<E>(E saveObject, string path)
        {
            SaveJSONString(JsonSerializer.Serialize(saveObject), path);
        }

        public static void DeleteFileAtPath(string path)
        {
            DirAccess.RemoveAbsolute(path);
        }

        /// <summary>
        /// Load object E from the specified path and return it.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static E LoadObject<E>(string path)
        {
            E loadObject = default(E);

            if (FileAccess.FileExists(path))
            {
                using (FileAccess gdFile = FileAccess.Open(path, FileAccess.ModeFlags.Read))
                {
                    loadObject = JsonSerializer.Deserialize<E>(gdFile.GetAsText());
                    gdFile.Close();
                }
            }
            
            return loadObject;
        }

        private static void SaveJSONString(string json, string path)
        {
            
            using (var gdFile = FileAccess.Open(path, FileAccess.ModeFlags.Write))
            {
                if(gdFile == null)
                {
                    throw new Exception($"Failed to open file at {path}");
                }
                gdFile.StoreString(json);
                gdFile.Close();
            }
        }
    }
}
