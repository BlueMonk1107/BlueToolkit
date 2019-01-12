using UnityEditor;
using UnityEngine;

namespace BlueToolkit
{
    public class AddNameSpaceWindow : EditorWindow
    {
        private static string _name;
        private static AddNameSpaceWindow _window;

        [MenuItem("BlueToolKit/自动添加命名空间")]
        public static void Window()
        {
            _window = (AddNameSpaceWindow)GetWindow(typeof(AddNameSpaceWindow));
            _window.minSize = new Vector2(500, 400);
            _window.Show();
            Init();
        }

        private static void Init()
        {
            _name = ToolCacheManager.GetCacheObject<NameSpaceData>(PathManager.NameSpaceDataPath).NameSpaceName;
        }

        private void OnGUI()
        {
            GUILayout.Label("命名空间名称");
            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(200));
            _name = EditorGUI.TextField(rect, _name);

            if (GUILayout.Button("完成", GUILayout.MaxWidth(100)))
            {
                NameSpaceData data = new NameSpaceData();
                data.NameSpaceName = _name;
                ToolCacheManager.SaveCacheObject(PathManager.NameSpaceDataPath, data);
                _window.Close();
            }
        }

    }
}
