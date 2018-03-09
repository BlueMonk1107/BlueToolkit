//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BlueToolkit
{
    public class UnityUIEvent
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            Action OnEvent = delegate
            {
                ChangeDefaultFont();
            };


            EditorApplication.hierarchyWindowChanged = delegate()
            {
                OnEvent();
            };
        }

        private static void ChangeDefaultFont()
        {
            if (Selection.activeGameObject != null)
            {
                Text text = Selection.activeGameObject.Get<Text>();
                if (text != null)
                {
                    text.font = ToolCacheManager.GetFont();
                }
            }
        }
    }
}