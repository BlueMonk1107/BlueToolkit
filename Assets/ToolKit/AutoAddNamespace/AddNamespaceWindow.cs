using UnityEngine;
using UnityEditor;
using System.IO;

namespace BlueToolkit
{
    /// <summary>
    /// 自动添加命名空间工具
    /// </summary>
    public class AddNamespaceWindow : EditorWindow
    {
        public static bool _isOn;

        [MenuItem("BlueToolKit/自动添加命名空间工具")]
        public static void OpenWindow()
        {
            var window = GetWindow(typeof (AddNamespaceWindow));
            window.minSize = new Vector2(500,300);
            window.Show();
            Init();
        }

        public static NamespaceData GetData()
        {
            return ToolCacheManager.GetCacheObject<NamespaceData>(PathManager.NameSpaceDataPath);
        }

        private static void Init()
        {
            NamespaceData data = GetData();
            if (data != null)
            {
                _isOn = data.IsOn;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("命名空间名称");
            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(200));

            _isOn = GUILayout.Toggle(_isOn, "是否开启插件");

            if(GUILayout.Button("完成", GUILayout.MaxWidth(100)))
            {
                NamespaceData data = new NamespaceData();
                data.IsOn = _isOn;

                ToolCacheManager.SaveCacheObject(PathManager.NameSpaceDataPath,data);
            }
        }
    }
}
