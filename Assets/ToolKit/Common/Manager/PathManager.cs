using UnityEngine;

namespace BlueToolkit
{
    public static class PathManager
    {
        /// <summary>
        /// 缓存路径
        /// </summary>
        public static readonly string CachePath = "Assets/ToolKit/Cache/";
        /// <summary>
        /// 字体插件缓存路径
        /// </summary>
        public static readonly string FontDataPath = CachePath + "FontData.asset";
        /// <summary>
        /// Entitas框架脚本一键生成工具缓存路径
        /// </summary>
        public static readonly string EntitasDataPath = CachePath + "EntitasData.asset";
        /// <summary>
        /// StrangeIOC框架脚本一键生成工具缓存路径
        /// </summary>
        public static readonly string StrangeIOCDataPath = CachePath + "StrangeIOC.asset";

        /// <summary>
        /// 自动添加命名空间插件缓存路径
        /// </summary>
        public static readonly string NameSpaceDataPath = CachePath + "Namespace.asset";
    }
}
