//=======================================================
// 作者：Snake
// 描述：自定义创建脚本生成器
////链接：https://www.jianshu.com/p/9dffc487f645
//=======================================================

using System.Text;

public class ScriptBuilder
{
    private const string NEW_LINE = "\r\n";
    public ScriptBuilder()
    {
        builder = new StringBuilder();
    }

    private StringBuilder builder;
    public int Indent { get; set; }

    private int currentCharIndex;

    public void Write(string val, bool noAutoIndent = false)
    {
        if (!noAutoIndent)
            val = GetIndents() + val;
        if (currentCharIndex == builder.Length)
            builder.Append(val);
        else
            builder.Insert(currentCharIndex, val);
        currentCharIndex += val.Length;
    }

    public void WriteLine(string val, bool noAutoIndent = false)
    {
        Write(val + NEW_LINE);
    }

    public void WriteCurlyBrackets()
    {
        var openBracket = GetIndents() + "{" + NEW_LINE;
        var closeBracket = GetIndents() + "}" + NEW_LINE;
        Write(openBracket + closeBracket, true);
        currentCharIndex -= closeBracket.Length;
    }

    public string GetIndents()
    {
        var str = "";
        for (var i = 0; i < Indent; i++)
            str += "    ";
        return str;
    }

    public override string ToString()
    {
        return builder.ToString();
    }
}
