using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace BlueToolkit
{
    /// <summary>
    /// StrangeIOC 框架一键生成脚本工具
    /// </summary>
    public class CreatFrameScript : EditorWindow
    {

        private static CreatFrameScript m_CreatFrameScript;

        private static string m_ScriptName;
        private static string m_NameSpaceNamePrefix = "Frame_";

        private static string m_ViewPath;
        private static string m_MediatorPath;

        private static bool m_IsCreatView;
        private static bool m_IsCreatMediator;

        private static string m_Viewkey = "View";
        private static string m_Mediatorkey = "Mediator";

        [MenuItem("BlueToolKit/StrangeIOC 框架一键生成脚本工具")]
        public static void Window()
        {
            m_CreatFrameScript = (CreatFrameScript) GetWindow(typeof (CreatFrameScript));
            m_CreatFrameScript.minSize = new Vector2(500, 400);
            m_CreatFrameScript.Show();
            GetPathFromLocation();
            Init();
        }

        private static void Init()
        {
            m_IsCreatView = false;
            m_IsCreatMediator = false;
            m_ScriptName = "";
        }

        private void OnGUI()
        {
            GUILayout.Label("脚本地址");
            CreatPathItem("View脚本地址", ref m_ViewPath);
            CreatPathItem("Mediator脚本地址", ref m_MediatorPath);

            if (GUILayout.Button("保存路径", GUILayout.MaxWidth(100)))
            {
                SavePathToLocation();
            }

            CreatItem("脚本名称（前缀）", ref m_ScriptName);

            ShowScriptName(ref m_IsCreatView, m_Viewkey);
            ShowScriptName(ref m_IsCreatMediator, m_Mediatorkey);



            if (GUILayout.Button("生成脚本", GUILayout.MaxWidth(100)))
            {
                CreateAllScripts();
                m_CreatFrameScript.Close();
            }
        }

        private void ShowScriptName(ref bool isSelected, string key)
        {
            GUILayout.BeginHorizontal();
            isSelected = GUILayout.Toggle(isSelected, "脚本名称：" + m_ScriptName + key);
            GUILayout.Label("命名空间：" + m_NameSpaceNamePrefix + key);
            GUILayout.EndHorizontal();
        }

        private void CreatPathItem(string name, ref string path)
        {
            Rect rect = CreatItem(name, ref path);
            DragToPath(rect, ref path);
        }

        private Rect CreatItem(string name, ref string context)
        {
            GUILayout.Label(name);
            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(200));
            context = EditorGUI.TextField(rect, context);
            return rect;
        }

        private void DragToPath(Rect rect, ref string path)
        {
            if ((Event.current.type == EventType.DragUpdated
                 || Event.current.type == EventType.DragExited)
                && rect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    path = DragAndDrop.paths[0];
                }
            }
        }

        private void SavePathToLocation()
        {
            ScriptsPathData data = new ScriptsPathData();
            data.ViewPath = m_ViewPath;
            data.MediatorPath = m_MediatorPath;
            ToolCacheManager.SaveCacheObject(PathManager.StrangeIOCDataPath, data);
        }

        private static void GetPathFromLocation()
        {
            ScriptsPathData data = ToolCacheManager.GetCacheObject<ScriptsPathData>(PathManager.StrangeIOCDataPath);
            m_ViewPath = data.ViewPath;
            m_MediatorPath = data.MediatorPath;
        }

        private void CreateAllScripts()
        {
            CreatScript(m_IsCreatView, m_Viewkey, m_ViewPath, GetViewCode());
            CreatScript(m_IsCreatMediator, m_Mediatorkey, m_MediatorPath, GetMediator());
        }

        private void CreatScript(bool isSelected, string key, string path, string context)
        {
            if (isSelected)
            {
                Debug.Log(m_ScriptName);
                string className = m_ScriptName + key;
                string filePath = path + "/" + className + ".cs";
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, context, Encoding.UTF8);
                    Debug.Log("生成" + className + "成功");
                }
            }
        }

        private string GetViewCode()
        {
            var script = new ScriptBuildHelp();
            script.WriteUsing("UnityEngine");
            script.WriteUsing(m_NameSpaceNamePrefix + m_Viewkey);
            script.WriteEmptyLine();
            script.WriteNamespace(m_NameSpaceNamePrefix + m_Viewkey);

            script.IndentTimes++;
            script.WriteClass(m_ScriptName + m_Viewkey, "ViewBase");

            script.IndentTimes++;
            script.WriteFun("Init");
            return script.ToString();
        }

        private string GetMediator()
        {
            var script = new ScriptBuildHelp();
            script.WriteUsing(m_NameSpaceNamePrefix + m_Viewkey);
            script.WriteUsing("strange.extensions.mediation.impl");
            script.WriteEmptyLine();
            script.WriteNamespace(m_NameSpaceNamePrefix + m_Mediatorkey);

            script.IndentTimes++;
            script.WriteClass(m_ScriptName + m_Mediatorkey, "Mediator");

            script.IndentTimes++;

            script.WriteProperty("Inject", m_ScriptName + m_Viewkey, m_NameSpaceNamePrefix + m_Viewkey);
            script.WriteFun("Init");
            return script.ToString();
        }
    }
}