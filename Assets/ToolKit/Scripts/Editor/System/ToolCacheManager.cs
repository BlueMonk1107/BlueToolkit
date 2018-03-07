//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;

namespace BlueToolkit
{
    public class ToolCacheManager
    {
        private static readonly string cachePath =
            Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "Library/BlueToolkitCache/";

        //[InitializeOnLoadMethod]
        private static void Init()
        {
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }
        }

        public static void SaveData(string fileName,object obj)
        {
            string dataPath = cachePath + fileName +".json";

            FileStream stream = File.Open(dataPath, FileMode.OpenOrCreate);
            byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj));
            stream.Write(data, 0, data.Length);
            stream.Close();
        }

        public static T GetData<T>(string fileName)
        {
            string dataPath = cachePath + fileName + ".json";

            if (File.Exists(dataPath))
            {
                string data = File.ReadAllText(dataPath);
                return JsonUtility.FromJson<T>(data);
            }
            else
            {
                return default(T);
            }
        }

        public static void SaveFont(Font font)
        {
            BlueToolkit.FontData data = ScriptableObject.CreateInstance<BlueToolkit.FontData>();
            data.defaultFont = font;
            AssetDatabase.CreateAsset(data, "Assets/ToolKit/Cache/FontData.asset");
        }

        public static Font GetFont()
        {
            FontData data = AssetDatabase.LoadAssetAtPath<FontData>("Assets/ToolKit/Cache/FontData.asset");
            return data.defaultFont;
        }
    }
}
