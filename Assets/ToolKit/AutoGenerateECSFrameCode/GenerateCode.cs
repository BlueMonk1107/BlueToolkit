using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BlueToolkit
{
    /// <summary>
    /// 生成代码部分
    /// </summary>
    public class GenerateCode     
    {
        /// <summary>
        /// 在ServiceManager中插入创建的service进行服务初始化
        /// </summary>
        public static void InitServices(string path)
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                int index = content.IndexOf("IInitService[] services =");
                int newIndex = content.IndexOf("new", index);
                content = content.Insert(newIndex, "new " + ToolData.ServiceName + ToolData.ServicePostfix + "(),\r                ");
                File.WriteAllText(path, content, Encoding.UTF8);
            }
            else
            {
                Debug.LogError("Service脚本不存在");
            }
        }

        /// <summary>
        /// 把创建的系统添加到对应Feature中进行初始化
        /// </summary>
        /// <param name="contextName"></param>
        /// <param name="className"></param>
        /// <param name="systemName"></param>
        public static void InitSystem(string contextName, string className, params string[] systemName)
        {
            string path = "";
            switch (contextName)
            {
                case "Game":
                    path = ToolData.GameFeaturePath;
                    break;
                case "Input":
                    path = ToolData.InputFeaturePath;
                    break;
            }

            if (string.IsNullOrEmpty(path))
                return;

            foreach (string s in systemName)
            {
                SetSystem(path, s, className);
            }
        }

        /// <summary>
        /// 把初始化内容插入到对应Feature
        /// </summary>
        /// <param name="contextName"></param>
        /// <param name="className"></param>
        /// <param name="systemName"></param>
        private static void SetSystem(string path, string systemName, string className)
        {

            string content = File.ReadAllText(path);
            int index = content.IndexOf("void " + systemName + "Fun(Contexts contexts)");
            if (index < 0)
            {
                Debug.LogError("未找到对应方法，系统名：" + systemName);
                return;
            }

            int startIndex = content.IndexOf("{", index);
            content = content.Insert(startIndex + 1, "\r            Add(new " + className + "(contexts));");
            File.WriteAllText(path, content, Encoding.UTF8);
        }

        /// <summary>
        /// 创建脚本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="className"></param>
        /// <param name="scriptContent"></param>
        public static void CreateScript(string path, string className, string scriptContent)
        {
            string filePath = path + "/" + className + ".cs";
            if (Directory.Exists(path))
            {
                if (!File.Exists(filePath))
                {
                    SaveFile(filePath, scriptContent);
                }
                else
                {
                    if (EditorUtility.DisplayDialog("文件已存在", "是否覆盖文件", "覆盖", "取消"))
                    {
                        SaveFile(filePath, scriptContent);
                    }
                }
            }
            else
            {
                Debug.LogError("目录:" + path + "不存在");
            }

            AssetDatabase.Refresh();
        }

        private static void SaveFile(string filePath, string scriptContent)
        {
            File.WriteAllText(filePath, scriptContent, Encoding.UTF8);
        }
    }
}
