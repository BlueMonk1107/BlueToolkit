using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BlueToolkit
{
    /// <summary>
    /// 自动生成脚本帮助类
    /// </summary>
    public class ScriptBuildHelp
    {
        private StringBuilder _stringBuilder;
        private string _lineBrake = "\r\n";
        private int currentIndex = 0;
        public int IndentTimes { get; set; }

        public ScriptBuildHelp()
        {
            _stringBuilder = new StringBuilder();
        }

        //写入内容
        private void Write(string context, bool needIndent = false)
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

        //写入行
        private void WriteLine(string context, bool needIndent = false)
        {
            Write(context + _lineBrake, needIndent);
        }

        //获取缩进
        private string GetIndent()
        {
            string indent = "";
            for (int i = 0; i < IndentTimes; i++)
            {
                indent += "    ";
            }
            return indent;
        }

        //写入大括号
        private int WriteCurlyBrackets()
        {
            var start = _lineBrake+GetIndent() + "{" + _lineBrake;
            var end = GetIndent() + "}" + _lineBrake;
            Write(start + end, true);
            return end.Length;
        }

        //写入引用
        public void WriteUsing(string nameSpaceName)
        {
            WriteLine("using "+ nameSpaceName + ";");
        }

        //写入空行
        public void WriteEmptyLine()
        {
            WriteLine("");
        }

        //写入命名空间
        public void WriteNamespace(string name)
        {
            Write("namespace "+ name);
            int length = WriteCurlyBrackets();
            currentIndex -= length;
        }

        //写入类 第一个参数是类名 第二个参数是父类名
        public void WriteClass(string name,string baseName = "MonoBehaviour")
        {
            Write("public class "+ name+" : "+ baseName + " ",true);
            int length = WriteCurlyBrackets();
            currentIndex -= length;
        }

        //写入属性 第一个参数是特性名 第二个参数是属性的类型名 第三个参数是属性的名称
        public void WriteProperty(string attributeName,string typeName, string name)
        {
            WriteEmptyLine();
            WriteLine(GetIndent()+"[" + attributeName + "]");
            WriteLine(GetIndent()+"public " + typeName +" "+ name+ " { get; set; }");
            WriteEmptyLine();
        }

        //写入方法
        public void WriteFun(string name,params string[] paraName)
        {
            StringBuilder temp = new StringBuilder();
            temp.Append("public void " + name + "()");
            if (paraName.Length > 0)
            {
                foreach (string s in paraName)
                {
                    temp.Insert(temp.Length - 1, s + ",");
                }
                temp.Remove(temp.Length - 2, 1);
            }
            Write(temp.ToString(), true);
            WriteCurlyBrackets();
        }

        //输出生成脚本的内容
        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
}
}
