using UnityEngine;

namespace BlueToolkit
{
    [System.Serializable]
    public class NamespaceData : ScriptableObject
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public bool IsOn;
    }
}
