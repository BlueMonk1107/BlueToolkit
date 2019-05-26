using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BlueToolkit
{

    public class AutoAddNameSpace : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string path)
        {
            if (!IsOn())
                return;
            
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string text = "";
                text += File.ReadAllText(path);
                string name = GetClassName(text);
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }
                string nameSpace = GetNameSpaceName(path);
                var newText = GetNewScriptContext(nameSpace,name);
                File.WriteAllText(path, newText);
            }
            AssetDatabase.Refresh();
        }

        public static NamespaceData GetData()
        {
            return AssetDatabase.LoadAssetAtPath<NamespaceData>(PathManager.NameSpaceDataPath);
        }

        private static bool IsOn()
        {
            NamespaceData data = GetData();
            if (data != null)
            {
                return data.IsOn;
            }

            return false;
        }

        //获取新的脚本内容
        private static string GetNewScriptContext(string nameSpace,string className)
        {
            var script = new ScriptBuildHelp();
            script.WriteUsing("UnityEngine");
            script.WriteEmptyLine();
            script.WriteNamespace(nameSpace);

            script.IndentTimes++;
            script.WriteClass(className, "MonoBehaviour");

            script.IndentTimes++;
            List<string> keys = new List<string>();
            keys.Add("void");
            script.WriteFun(keys, "Start");
            return script.ToString();
        }

        //获取命名空间名称
        private static string GetNameSpaceName(string path)
        {
            StringBuilder nameSpace = new StringBuilder();

            string[] folders = path.Split('/');
            int startIndex = 0;
            for (int i = 0; i < folders.Length; i++)
            {
                if (folders[i] == "Assets")
                {
                    startIndex = i + 1;
                    break;
                }
            }

            for (int i = startIndex; i < folders.Length -1; i++)
            {
                nameSpace.Append(folders[i]);
                nameSpace.Append(".");
            }

            nameSpace.Remove(nameSpace.Length - 1, 1);

            return nameSpace.ToString();
        }

        //获取类名
        private static string GetClassName(string text)
        {
            string patterm = "public class ([A-Za-z0-9_]+)\\s*:\\s*MonoBehaviour";
            var match = Regex.Match(text, patterm);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return "";
        }
    }
}

