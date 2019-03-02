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
    /// <summary>
    /// 工具缓存管理类
    /// </summary>
    [InitializeOnLoad]
    public class ToolCacheManager
    {
        static ToolCacheManager()
        {
            CreateFolder(Application.streamingAssetsPath);
            CreateFolder(PathManager.CachePath);
        }

        private static void CreateFolder(string path)
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
        }

        /// <summary>
        /// 保存成Json数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public static void SaveData(string fileName,object obj)
        {
            string dataPath = Application.streamingAssetsPath + fileName +".json";

            FileStream stream = File.Open(dataPath, FileMode.OpenOrCreate);
            byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj));
            stream.Write(data, 0, data.Length);
            stream.Close();
        }

        /// <summary>
        /// 读取Json数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public static T GetData<T>(string fileName)
        {
            string dataPath = Application.streamingAssetsPath + fileName + ".json";

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
        /// 保存缓存数据对象(asset对象)
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheFilePath"></param>
        /// <param name="data"></param>
        public static void SaveCacheObject<TData>(string cacheFilePath, TData data) where TData : Object
        {
            AssetDatabase.CreateAsset(data, cacheFilePath);
        }

        /// <summary>
        /// 获取缓存数据对象(asset对象)
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
                    Debug.LogError("类型错误");
                    return null;
                }
                else
                {
                    return data;
                }
            }
            else
            {
                Debug.LogError("未找到缓存文件，路径：" + cacheFilePath);
                return null;
            }
        }
    }
}
