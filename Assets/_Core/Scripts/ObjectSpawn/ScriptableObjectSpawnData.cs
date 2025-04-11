using System.Collections.Generic;
using UnityEngine;


namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "ObjectSpawnScriptableData", menuName = "Scriptable Objects/ObjectSpawnScriptableData")]
    public class ScriptableObjectSpawnData : ScriptableObject
    {
        public List<ObjectSpawned> objectSpawnedList = new List<ObjectSpawned>();
        
        [System.Serializable]
        public struct ObjectSpawned
        {
            public Vector3 _verticesSpawnPosition;
            public GameObject _object;

            public ObjectSpawned(GameObject obj, Vector3 position)
            {
                _object = obj;
                _verticesSpawnPosition = position;
            }
        }
    }
}
