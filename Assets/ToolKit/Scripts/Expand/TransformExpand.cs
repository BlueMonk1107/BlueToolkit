//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================
using UnityEngine;

namespace BlueToolkit
{
    public static class TransformExpand
    {
        #region 设置位置
        public static Transform SetPositionX(this Transform trans, float x)
        {
            Vector3 temp = trans.position;
            temp.x = x;
            trans.position = temp;
            return trans;
        }

        public static Transform SetPositionY(this Transform trans, float y)
        {
            Vector3 temp = trans.position;
            temp.y = y;
            trans.position = temp;
            return trans;
        }
        public static Transform SetPositionZ(this Transform trans, float z)
        {
            Vector3 temp = trans.position;
            temp.z = z;
            trans.position = temp;
            return trans;
        }

        public static Transform SetLocalPositionX(this Transform trans, float x)
        {
            Vector3 temp = trans.localPosition;
            temp.x = x;
            trans.localPosition = temp;
            return trans;
        }
        public static Transform SetLocalPositionY(this Transform trans, float y)
        {
            Vector3 temp = trans.localPosition;
            temp.y = y;
            trans.localPosition = temp;
            return trans;
        }
        public static Transform SetLocalPositionZ(this Transform trans, float z)
        {
            Vector3 temp = trans.localPosition;
            temp.z = z;
            trans.localPosition = temp;
            return trans;
        }
        #endregion 设置位置

        #region 设置欧拉角
        public static Transform SetEulerAnglesX(this Transform trans, float x)
        {
            Vector3 temp = trans.eulerAngles;
            temp.x = x;
            trans.eulerAngles = temp;
            return trans;
        }
        public static Transform SetEulerAnglesY(this Transform trans, float y)
        {
            Vector3 temp = trans.eulerAngles;
            temp.y = y;
            trans.eulerAngles = temp;
            return trans;
        }
        public static Transform SetEulerAnglesZ(this Transform trans, float z)
        {
            Vector3 temp = trans.eulerAngles;
            temp.z = z;
            trans.eulerAngles = temp;
            return trans;
        }

        public static Transform SetLocalEulerAnglesX(this Transform trans, float x)
        {
            Vector3 temp = trans.localEulerAngles;
            temp.x = x;
            trans.localEulerAngles = temp;
            return trans;
        }
        public static Transform SetLocalEulerAnglesY(this Transform trans, float y)
        {
            Vector3 temp = trans.localEulerAngles;
            temp.y = y;
            trans.localEulerAngles = temp;
            return trans;
        }
        public static Transform SetLocalEulerAnglesZ(this Transform trans, float z)
        {
            Vector3 temp = trans.localEulerAngles;
            temp.z = z;
            trans.localEulerAngles = temp;
            return trans;
        }
        #endregion 设置欧拉角

        #region 设置缩放
        public static Transform SetScaleX(this Transform trans, float x)
        {
            Vector3 temp = trans.localScale;
            temp.x = x;
            trans.localScale = temp;
            return trans;
        }
        public static Transform SetScaleY(this Transform trans, float y)
        {
            Vector3 temp = trans.localScale;
            temp.y = y;
            trans.localScale = temp;
            return trans;
        }
        public static Transform SetScaleZ(this Transform trans, float z)
        {
            Vector3 temp = trans.localScale;
            temp.z = z;
            trans.localScale = temp;
            return trans;
        }
        #endregion 设置缩放

        public static T Get<T>(this Transform trans) where T:MonoBehaviour
        {
            if (trans.GetComponent<T>() != null)
            {
                return trans.GetComponent<T>();
            }
            else if(trans.GetComponentInChildren<T>() != null)
            {
                return trans.GetComponentInChildren<T>();
            }
            else if (trans.GetComponentInParent<T>() != null)
            {
                return trans.GetComponentInParent<T>();
            }
            else
            {
                return null;
            }
        }

        public static T Get<T>(this GameObject trans) where T : MonoBehaviour
        {
            if (trans.GetComponent<T>() != null)
            {
                return trans.GetComponent<T>();
            }
            else if (trans.GetComponentInChildren<T>() != null)
            {
                return trans.GetComponentInChildren<T>();
            }
            else if (trans.GetComponentInParent<T>() != null)
            {
                return trans.GetComponentInParent<T>();
            }
            else
            {
                return null;
            }
        }
    }

}
