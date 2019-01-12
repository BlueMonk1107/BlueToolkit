//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================

using UnityEngine;
using UnityEditor;

namespace BlueToolkit
{
    public class SetDefaultFont : EditorWindow
    {
        private static Font m_font;
        private static EditorWindow window;

        public static Font Font
        {
            get
            {
                return m_font;
            }
        }

        [MenuItem("BlueToolKit/设置默认字体")]
        public static void OpenWindow()
        {
            window = GetWindow(typeof (SetDefaultFont));
            window.minSize = new Vector2(500, 300);
            m_font = ToolCacheManager.GetCacheObject<FontData>(PathManager.FontDataPath).defaultFont;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("选择默认字体");
            EditorGUILayout.Space();
            m_font = (Font)EditorGUILayout.ObjectField(m_font, typeof (Font), true);
            EditorGUILayout.Space();
            if (GUILayout.Button("确定"))
            {
                FontData data = new FontData();
                data.defaultFont = m_font;
                ToolCacheManager.SaveCacheObject(PathManager.FontDataPath,data);
                window.Close();
            }
        }
    }
}