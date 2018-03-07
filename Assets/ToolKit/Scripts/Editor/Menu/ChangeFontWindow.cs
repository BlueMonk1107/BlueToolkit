//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace BlueToolkit
{
    public class ChangeFontWindow : EditorWindow
    {
        private static string path;
        private static Rect rect;
        private static GUIStyle style;
        private Font toChange;
        private static Font toChangeFont;
        private FontStyle toFontStyle;
        private static FontStyle toChangeFontStyle;

        [MenuItem("CustomTool/更换字体")]
        public static void OpenWindow()
        {
            EditorWindow window = GetWindow(typeof(ChangeFontWindow));
            window.minSize = new Vector2(500, 300);
            style = new GUIStyle();
            style.normal.textColor = Color.red;
        }

        void OnGUI()
        {
            CreatPathRect();

            if (SelectFolder())
            {
                path = EditorGUI.TextField(rect, path);

                SetFont();

                if (GUILayout.Button("更换"))
                {
                    Change();
                }
            }
            else
            {
                EditorGUILayout.LabelField("请选择Resources文件夹下路径", style);
            }
        }

        private void DragFolder()
        {
            //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内  
            if ((Event.current.type == EventType.DragUpdated
              || Event.current.type == EventType.DragExited)
              && rect.Contains(Event.current.mousePosition))
            {
                //改变鼠标的外表  
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    path = DragAndDrop.paths[0];
                }
            }
        }

        private void CreatPathRect()
        {
            EditorGUILayout.LabelField("路径");
            rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
            EditorGUI.TextField(rect, path);
        }

        private bool SelectFolder()
        {
            UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);

            if (arr.Length > 0)
            {
                path = AssetDatabase.GetAssetPath(arr[0]);
                if (path.Contains("Resources"))
                {
                    path = path.Substring(path.IndexOf("Resources")).Substring(10);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void SetFont()
        {
            toChange = (Font)EditorGUILayout.ObjectField(toChange, typeof(Font), true, GUILayout.MinWidth(100f));
            toChangeFont = toChange;
            toFontStyle = (FontStyle)EditorGUILayout.EnumPopup(toFontStyle, GUILayout.MinWidth(100f));
            toChangeFontStyle = toFontStyle;
        }

        public static void Change()
        {
            foreach (var item in Resources.LoadAll<GameObject>(path))
            {
                Text[] textList = item.GetComponentsInChildren<Text>();

                foreach (var t in textList)
                {
                    Undo.RecordObject(t, t.gameObject.name);
                    t.font = toChangeFont;
                    t.fontStyle = toChangeFontStyle;
                    EditorUtility.SetDirty(t);
                }
                Debug.Log("修改字体成功");
            }
        }
    }

}