//作者：BowenK
//描述：创建脚本自动添加命名空间
//链接：https://www.jianshu.com/p/9dffc487f645
//=======================================================
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


namespace Babybus.Uno
{
    public class AutoAddNamespace : UnityEditor.AssetModificationProcessor
    {

        private static readonly string AuthorCode =
        "//=======================================================\r\n"
        + "// 作者：BlueMonk\r\n"
        + "// 描述：A set of tools designed to increase the efficiency of unity development. \r\n"
        + "//=======================================================\r\n";

        public static readonly string headCode =
        "using UnityEngine;\r\n"
        + "using System.Collections;\r\n"
        + "\r\n";

        public static void OnWillCreateAsset(string path)
        {
            //只修改C#脚本
            var scriptName = "";
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string allText = "";
                allText += File.ReadAllText(path);
                scriptName = GetClassName(allText);
                if (scriptName != "")
                {
                    CreateClass(path, scriptName);
                }
            }
        }

        //创建新的类 
        public static void CreateClass(string path, string className)
        {
            var sb = new ScriptBuilder();
            //添加命名空间
            sb.WriteLine("namespace BlueToolkit");
            sb.WriteCurlyBrackets();
            sb.Indent++;

            sb.WriteLine("public class #SCRIPTNAME# : MonoBehaviour");
            sb.WriteCurlyBrackets();
            sb.Indent++;

            var allText = AuthorCode + headCode + sb.ToString();
            //替换脚本名字
            allText = allText.Replace("#SCRIPTNAME#", className);
            File.WriteAllText(path, allText);
        }

        //首字母改成大写
        public static string FirstLetterUppercase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            if (str.Length == 1)
                return str.ToUpper();

            var first = str[0];
            var rest = str.Substring(1);
            return first.ToString().ToUpper() + rest;
        }

        //获取unity自动创建的脚本类名
        public static string GetClassName(string allText)
        {
            var patterm = "public class ([A-Za-z0-9_]+)\\s*:\\s*MonoBehaviour {\\s*\\/\\/ Use this for initialization\\s*void Start \\(\\) {\\s*}\\s*\\/\\/ Update is called once per frame\\s*void Update \\(\\) {\\s*}\\s*}";
            var match = Regex.Match(allText, patterm);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "";
        }

    }
}