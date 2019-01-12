using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueToolkit
{
    [System.Serializable]
    public class ScriptsPathData : ScriptableObject
    {
        [SerializeField]
        public string ViewPath;
        [SerializeField]
        public string MediatorPath;
    }
}