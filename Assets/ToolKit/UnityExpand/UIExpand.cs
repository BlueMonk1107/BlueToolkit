//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================
using UnityEngine;
using UnityEngine.UI;

namespace BlueToolkit
{
    public static class UIExpand
    {
        public static RectTransform RectTransform(this Transform trans)
        {
            return trans.GetComponent<RectTransform>();
        }
        
        public static Image Image(this Transform trans)
        {
            return trans.GetComponent<Image>();
        }

        public static Text Text(this Transform trans)
        {
            return trans.GetComponent<Text>();
        }

        public static Vector2 SetSizeDeltaX(this Transform trans,float x)
        {
            Vector2 temp = trans.RectTransform().sizeDelta;
            temp.x = x;
            trans.RectTransform().sizeDelta = temp;
            return trans.RectTransform().sizeDelta;
        }

        public static Vector2 SetSizeDeltaY(this Transform trans, float y)
        {
            Vector2 temp = trans.RectTransform().sizeDelta;
            temp.y = y;
            trans.RectTransform().sizeDelta = temp;
            return trans.RectTransform().sizeDelta;
        }
    }
}
