using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BlueToolkit
{
    public class ScriptBuildHelp
    {
        public static string Public = "public";
        public static string Private = "private";
        public static string Protected = "protected";
         
        private StringBuilder _stringBuilder;
        private string _lineBrake = "\r\n";
        private int currentIndex = 0;
        public int IndentTimes { get; set; }

        /// <summary>
        /// 回到大括号中间，需要缩进的值
        /// </summary>
        private int _backNum
        {
            get { return (GetIndent() + "}" + _lineBrake).Length; }
        }

        public ScriptBuildHelp()
        {
            _stringBuilder = new StringBuilder();
            ResetData();
        }

        public void ResetData()
        {
            _stringBuilder.Clear();
            currentIndex = 0;
        }

        private void Write(string context, bool needIndent = true)
        {
            if (needIndent)
            {
                context = GetIndent() + context;
            }

            if (currentIndex == _stringBuilder.Length)
            {
                _stringBuilder.Append(context);
            }
            else
            {
                _stringBuilder.Insert(currentIndex, context);
            }

            currentIndex += context.Length;
        }

        public void WriteLine(string context, bool needIndent = false)
        {
            Write(context + _lineBrake, needIndent);
        }

        private string GetIndent()
        {
            string indent = "";
            for (int i = 0; i < IndentTimes; i++)
            {
                indent += "    ";
            }
            return indent;
        }

        /// <summary>
        /// 返回值为回到大括号中间，需要缩进的值
        /// </summary>
        /// <returns></returns>
        private int WriteCurlyBrackets()
        {
            var start = _lineBrake+GetIndent() + "{" + _lineBrake;
            var end = GetIndent() + "}" + _lineBrake;
            Write(start + end);
            return end.Length;
        }

        public void WriteUsing(string nameSpaceName)
        {
            WriteLine("using "+ nameSpaceName + ";");
        }

        public void WriteEmptyLine()
        {
            WriteLine("");
        }

        public void WriteNamespace(string name)
        {
            Write("namespace "+ name);
            WriteCurlyBrackets();
            BackToInsertContent();
        }

        public void WriteClass(string name,params string[] baseName)
        {
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < baseName.Length; i++)
            {
                temp.Append(baseName[i]);
                if (i != baseName.Length - 1)
                {
                    temp.Append(",");
                }
            }

            Write("public class "+ name+" : "+ temp+ " ");
            WriteCurlyBrackets();
            BackToInsertContent();
        }

        //写入属性 第一个参数是特性名 第二个参数是属性的类型名 第三个参数是属性的名称
        public void WriteProperty(string attributeName, string typeName, string name)
        {
            WriteEmptyLine();
            WriteLine(GetIndent() + "[" + attributeName + "]");
            WriteLine(GetIndent() + "public " + typeName + " " + name + " { get; set; }");
            WriteEmptyLine();
        }

        public void WriteInterface(string name, params string[] baseName)
        {
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < baseName.Length; i++)
            {
                temp.Append(baseName[i]);
                if (i != baseName.Length - 1)
                {
                    temp.Append(",");
                }
            }

            Write("public interface " + name + " : " + temp + " ", true);
            WriteCurlyBrackets();
            BackToInsertContent();
        }

        public void WriteFun(List<string> keyName,string name,string othes = "",params string[] paraName)
        {
            WriteFun(name, Public, keyName, othes, paraName);
        }

        public void WriteFun(string name, string publicState = "public", List<string> keyName = null, string othes = "", params string[] paraName)
        {
            StringBuilder keyTemp = new StringBuilder();

            if (keyName != null)
            {
                for (int i = 0; i < keyName.Count; i++)
                {
                    keyTemp.Append(keyName[i]);
                    if (i != keyName.Count - 1)
                    {
                        keyTemp.Append(" ");
                    }
                }

            }

            StringBuilder temp = new StringBuilder();
            temp.Append(publicState + " " + keyTemp + " " + name + "()");
            if (paraName.Length > 0)
            {
                foreach (string s in paraName)
                {
                    temp.Insert(temp.Length - 1, s + ",");
                }
                temp.Remove(temp.Length - 2, 1);
            }

            temp.Append(" ");
            temp.Append(othes);

            Write(temp.ToString());
            WriteCurlyBrackets();
        }

        /// <summary>
        /// 设置光标位置,到大括号内插入内容
        /// </summary>
        /// <param name="num"></param>
        public void BackToInsertContent()
        {
            currentIndex -= _backNum;
        }

        /// <summary>
        /// 设置光标位置,到结束大括号外
        /// </summary>
        /// <param name="num"></param>
        public void ToContentEnd()
        {
            currentIndex += _backNum;
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
