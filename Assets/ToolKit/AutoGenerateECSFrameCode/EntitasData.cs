using UnityEngine;

namespace BlueToolkit
{
    [System.Serializable]
    public class EntitasData : ScriptableObject     
    {
        /// <summary>
        /// View层路径
        /// </summary>
        public string ViewPath;
        /// <summary>
        /// Service层路径
        /// </summary>
        public string ServicePath;
        /// <summary>
        /// System层路径
        /// </summary>
        public string SystemPath;
        /// <summary>
        /// ServiceManager路径
        /// </summary>
        public string ServiceManagerPath;
        /// <summary>
        /// ViewFeature脚本路径
        /// </summary>
        public string ViewFeaturePath;
        /// <summary>
        /// InputFeature脚本路径
        /// </summary>
        public string InputFeaturePath;
        /// <summary>
        /// GameFeature脚本路径
        /// </summary>
        public string GameFeaturePath;
    }
}
