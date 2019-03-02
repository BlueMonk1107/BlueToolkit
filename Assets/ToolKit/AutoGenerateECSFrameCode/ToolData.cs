using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesperateDevs.Serialization;
using Entitas.CodeGeneration.Plugins;
using UnityEditor;
using UnityEngine;

namespace BlueToolkit
{
    /// <summary>
    /// 工具数据类
    /// </summary>
    public class ToolData      
    {
        /// <summary>
        /// view脚本存放路径
        /// </summary>
        public static string ViewPath;
        /// <summary>
        /// service脚本存放路径
        /// </summary>
        public static string ServicePath;
        /// <summary>
        /// serviceManager脚本路径
        /// </summary>
        public static string ServiceManagerPath;
        /// <summary>
        /// 系统脚本存放路径
        /// </summary>
        public static string SystemPath;
        /// <summary>
        /// View层脚本后缀
        /// </summary>
        public static string ViewPostfix = "View";
        /// <summary>
        /// 用户输入的View层脚本名称
        /// </summary>
        public static string ViewName;
        /// <summary>
        /// Service层脚本后缀
        /// </summary>
        public static string ServicePostfix = "Service";
        /// <summary>
        /// 用户输入的Service层脚本名称
        /// </summary>
        public static string ServiceName;
        /// <summary>
        /// System层脚本后缀
        /// </summary>
        public static string SystemPostfix = "System";
        /// <summary>
        /// 输入的响应系统脚本名称
        /// </summary>
        public static string ReactiveSystemName;
        /// <summary>
        /// 基础命名空间
        /// </summary>
        public static string NamespaceBase = "Game";
        /// <summary>
        /// 当前已有的上下文数组
        /// </summary>
        public static string[] ContextNames;
        /// <summary>
        /// 每个上下文的选中状态 key：上下文名称 value：是否选择（true为选中）
        /// </summary>
        public static Dictionary<string, bool> ContextSelectedState;
        /// <summary>
        /// 当前选中上下文名称
        /// </summary>
        public static string SelectedContextName;

        /// <summary>
        /// ViewFeature脚本路径
        /// </summary>
        public static string ViewFeaturePath;
        /// <summary>
        ///  InputFeature脚本路径
        /// </summary>
        public static string InputFeaturePath;
        /// <summary>
        ///  GameFeature脚本路径
        /// </summary>
        public static string GameFeaturePath;

        /// <summary>
        /// 其他系统输入的名称
        /// </summary>
        public static string OtherSystemName;
        /// <summary>
        /// 其他系统接口名称数组
        /// </summary>
        public static string[] SystemInterfaceName =
        {
            "IInitializeSystem",
            "IExecuteSystem",
            "ICleanupSystem",
            "ITearDownSystem"
        };
        /// <summary>
        /// 系统选择状态缓存 key：系统名称 value：是否选择（true为选中）
        /// </summary>
        public static Dictionary<string, bool> SystemSelectedState;

        public static void Init()
        {
            GetContextName();
            ReadDataFromLocal();
            InitContextSelectedState();
            SelectedContextName = ContextNames[0];
            InitSystemSelectedState();
        }

        private static void InitContextSelectedState()
        {
            ContextSelectedState = new Dictionary<string, bool>();

            ResetContextSelectedState();
        }

        private static void InitSystemSelectedState()
        {
            SystemSelectedState = new Dictionary<string, bool>();
            foreach (string system in SystemInterfaceName)
            {
                SystemSelectedState[system] = false;
            }
        }

        public static void ResetContextSelectedState()
        {
            foreach (string contextName in ContextNames)
            {
                ContextSelectedState[contextName] = false;
            }
        }

        /// <summary>
        /// 获取所有上下文名称
        /// </summary>
        private static void GetContextName()
        {
            var provider = new ContextDataProvider();
            provider.Configure(Preferences.sharedInstance);
            var data = (ContextData[])provider.GetData();
            ContextNames = data.Select(u => u.GetContextName()).ToArray();
        }

        //保存数据到本地
        public static void SaveDataToLocal()
        {
            EntitasData data = new EntitasData();
            data.ViewPath = ViewPath;
            data.ServicePath = ServicePath;
            data.SystemPath = SystemPath;
            data.ServiceManagerPath = ServiceManagerPath;
            data.GameFeaturePath = GameFeaturePath;
            data.InputFeaturePath = InputFeaturePath;
            data.ViewFeaturePath = ViewFeaturePath;
            ToolCacheManager.SaveCacheObject(PathManager.EntitasDataPath, data);
        }

        //从本地读取数据
        private static void ReadDataFromLocal()
        {
            EntitasData data = ToolCacheManager.GetCacheObject<EntitasData>(PathManager.EntitasDataPath);
            if (data != null)
            {
                ViewPath = data.ViewPath;
                ServicePath = data.ServicePath;
                SystemPath = data.SystemPath;
                ServiceManagerPath = data.ServiceManagerPath;
                GameFeaturePath = data.GameFeaturePath;
                InputFeaturePath = data.InputFeaturePath;
                ViewFeaturePath = data.ViewFeaturePath;
            }
        }
    }
}
