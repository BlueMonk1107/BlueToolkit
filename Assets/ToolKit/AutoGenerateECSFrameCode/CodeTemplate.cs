using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BlueToolkit
{
    /// <summary>
    /// 代码模板
    /// </summary>
    public class CodeTemplate     
    {
        /// <summary>
        /// 获取VIew层代码模板
        /// </summary>
        /// <returns></returns>
        public static string GetViewCode()
        {
            var build = new ScriptBuildHelp();
            build.WriteUsing("Entitas");
            build.WriteUsing("Entitas.Unity");
            build.WriteEmptyLine();

            build.WriteNamespace(ToolData.NamespaceBase + "." + ToolData.ViewPostfix);

            build.IndentTimes++;
            build.WriteClass(ToolData.ViewName + ToolData.ViewPostfix, "ViewBase");

            build.IndentTimes++;
            List<string> keyName = new List<string>();
            keyName.Add("override");
            keyName.Add("void");
            build.WriteFun(keyName, "Init", "", "Contexts contexts", "IEntity entity");

            build.BackToInsertContent();
            build.IndentTimes++;
            build.WriteLine(" base.Init(contexts, entity);", true);
            build.ToContentEnd();

            return build.ToString();
        }

        /// <summary>
        /// 获取Service层代码模板
        /// </summary>
        /// <returns></returns>
        public static string GetServiceCode()
        {
            string className = ToolData.ServiceName + ToolData.ServicePostfix;

            var build = new ScriptBuildHelp();
            build.WriteNamespace(ToolData.NamespaceBase + "." + ToolData.ServicePostfix);
            //interface
            build.IndentTimes++;
            build.WriteInterface("I" + className, "IInitService");
            build.ToContentEnd();
            //class
            build.WriteClass(className, "I" + className);
            //init函数
            build.IndentTimes++;
            List<string> initKey = new List<string>();
            initKey.Add("void");
            build.WriteFun(initKey, "Init", "", "Contexts contexts");
            //init 内容
            build.BackToInsertContent();
            build.IndentTimes++;
            build.WriteLine("//contexts.service.SetGameService" + className + "(this);", true);
            build.IndentTimes--;
            build.ToContentEnd();
            //GetPriority函数
            var key = new List<string>();
            key.Add("int");
            build.WriteFun(key, "GetPriority");

            build.BackToInsertContent();
            build.IndentTimes++;
            build.WriteLine("return 0;", true);
            build.ToContentEnd();

            return build.ToString();
        }

        /// <summary>
        /// 获取ReactiveSystem层代码模板
        /// </summary>
        /// <returns></returns>
        public static string GetReactiveSystemCode()
        {
            string className = ToolData.SelectedContextName + ToolData.ReactiveSystemName + "Reactive" + ToolData.SystemPostfix;
            string entityName = ToolData.SelectedContextName + "Entity";

            var build = new ScriptBuildHelp();
            build.WriteUsing("Entitas");
            build.WriteUsing("System.Collections.Generic");
            build.WriteNamespace(ToolData.NamespaceBase);

            build.IndentTimes++;
            build.WriteClass(className, "ReactiveSystem<" + entityName + ">");

            build.IndentTimes++;
            build.WriteLine(" protected Contexts _contexts;", true);
            build.WriteEmptyLine();
            //构造
            build.WriteFun(new List<string>(), className, " : base(context."+ ToolData.SelectedContextName.ToLower() + ")", "Contexts context");
            build.BackToInsertContent();
            build.IndentTimes++;
            build.WriteLine(" _contexts = context;", true);
            build.IndentTimes--;
            build.ToContentEnd();
            //GetTrigger
            List<string> triggerkeys = new List<string>();
            triggerkeys.Add("override");
            triggerkeys.Add("ICollector<" + entityName + ">");
            build.WriteFun("GetTrigger", ScriptBuildHelp.Protected, triggerkeys, "", "IContext<" + entityName + "> context");
            build.BackToInsertContent();
            build.IndentTimes++;
            build.WriteLine("return context.CreateCollector(" + ToolData.SelectedContextName + "Matcher." + ToolData.SelectedContextName + ToolData.ReactiveSystemName + ");", true);
            build.IndentTimes--;
            build.ToContentEnd();


            //Filter
            List<string> filerkeys = new List<string>();
            filerkeys.Add("override");
            filerkeys.Add("bool");
            build.WriteFun("Filter", ScriptBuildHelp.Protected, filerkeys, "", entityName + " entity");
            build.BackToInsertContent();
            build.IndentTimes++;
            build.WriteLine("return entity.has" + ToolData.SelectedContextName + ToolData.ReactiveSystemName + ";", true);
            build.IndentTimes--;
            build.ToContentEnd();


            //Execute
            List<string> executeKeys = new List<string>();
            executeKeys.Add("override");
            executeKeys.Add("void");
            build.WriteFun("Execute", ScriptBuildHelp.Protected, executeKeys, "", "List<" + entityName + "> entities");

            return build.ToString();
        }

        /// <summary>
        /// 获取其他系统代码模板
        /// </summary>
        /// <returns></returns>
        public static string GetOthersSystemCode()
        {
            string className = ToolData.SelectedContextName + ToolData.OtherSystemName + ToolData.SystemPostfix;
            List<string> selectedSystem = GetSelectedSysytem();

            var build = new ScriptBuildHelp();
            build.WriteUsing("Entitas");
            build.WriteNamespace(ToolData.NamespaceBase);

            build.IndentTimes++;
            build.WriteClass(className, GetSelectedSystemInterface(selectedSystem));

            build.IndentTimes++;
            build.WriteLine("protected Contexts _contexts;", true);
            build.WriteEmptyLine();
            //构造
            build.WriteFun(new List<string>(), className, "", "Contexts context");
            build.BackToInsertContent();
            build.IndentTimes++;
            build.WriteLine("_contexts = context;", true);
            build.IndentTimes--;
            build.ToContentEnd();
            //实现方法
            List<string> funName = GetFunName(selectedSystem);
            List<string> key = new List<string>();
            key.Add("void");
            foreach (string fun in funName)
            {
                build.WriteFun(key, fun);
            }
            return build.ToString();
        }

        /// <summary>
        /// 获取选中的系统接口字符串
        /// </summary>
        /// <returns></returns>
        private static string GetSelectedSystemInterface(List<string> selected)
        {
            StringBuilder temp = new StringBuilder();

            foreach (string name in selected)
            {
                temp.Append(name);
                temp.Append(" , ");
            }

            temp.Remove(temp.Length - 2, 2);

            return temp.ToString();
        }

        /// <summary>
        /// 获取选中的系统接口名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSelectedSysytem()
        {
            List<string> temp = new List<string>();
            foreach (KeyValuePair<string, bool> pair in ToolData.SystemSelectedState)
            {
                if (pair.Value)
                {
                    temp.Add(pair.Key);
                }
            }

            return temp;
        }

        /// <summary>
        /// 获取系统接口对应的实现方法名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFunName(List<string> selected)
        {
            List<string> temp = new List<string>();

            foreach (string interfaceName in selected)
            {
                temp.Add(interfaceName.Substring(1, interfaceName.Length - 7));
            }

            return temp;
        }
    }
}
