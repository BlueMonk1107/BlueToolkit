//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================
using UnityEngine;
using System.Collections;
using System;

namespace BlueToolkit
{
    [System.Serializable]
    public class FontData : ScriptableObject
    {
        [SerializeField]
        public Font defaultFont;
    }
}
