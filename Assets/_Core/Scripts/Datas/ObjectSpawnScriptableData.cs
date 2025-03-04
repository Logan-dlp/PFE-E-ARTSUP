using UnityEngine;


namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "ObjectSpawnScriptableData", menuName = "Scriptable Objects/ObjectSpawnScriptableData")]
    public class ObjectSpawnScriptableData : ScriptableObject
    {
        public ObjectSpwaned[] objectSpawneds;
        
        [System.Serializable]
        public struct ObjectSpwaned
        {
            public Vector3 _verticesSpawnPosition;
            public GameObject _object;

        }
    }
}
