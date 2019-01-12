using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BlueToolkit
{
    /// <summary>
    /// 自动添加命名空间工具
    /// </summary>
    public class AutoAddNameSpace : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string path)
        {
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
                var newText = GetNewScriptContext(name);
                File.WriteAllText(path, newText);
            }
            AssetDatabase.Refresh();
        }

        //获取新的脚本内容
        private static string GetNewScriptContext(string className)
        {
            var script = new ScriptBuildHelp();
            script.WriteUsing("UnityEngine");
            script.WriteEmptyLine();
            script.WriteNamespace(GetNamespaceName());

            script.IndentTimes++;
            script.WriteClass(className);
            return script.ToString();
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

        private static string GetNamespaceName()
        {
            return ToolCacheManager.GetCacheObject<NameSpaceData>(PathManager.NameSpaceDataPath).NameSpaceName;
        }
    }
}

