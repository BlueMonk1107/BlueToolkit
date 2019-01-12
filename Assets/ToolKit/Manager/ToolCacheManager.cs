//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================

using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using Object = UnityEngine.Object;

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

        /// <summary>
        /// 保存缓存数据对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheFilePath"></param>
        /// <param name="data"></param>
        public static void SaveCacheObject<TData>(string cacheFilePath, TData data) where TData : Object
        {
            AssetDatabase.CreateAsset(data, cacheFilePath);
        }

        /// <summary>
        /// 获取缓存数据对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheFilePath"></param>
        /// <returns></returns>
        public static TData GetCacheObject<TData>(string cacheFilePath) where TData : Object
        {
            if (File.Exists(cacheFilePath))
            {
                TData data = AssetDatabase.LoadAssetAtPath<TData>(cacheFilePath);
                if (data == null)
                {
                    throw new Exception("类型错误");
                }
                else
                {
                    return data;
                }
            }
            else
            {
                throw new Exception("未找到缓存文件，路径：" + cacheFilePath);
            }
        }
    }
}
