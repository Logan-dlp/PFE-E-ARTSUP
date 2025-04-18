using System.Collections.Generic;
using MoonlitMixes.Datas;
using UnityEditor;
using UnityEngine;

namespace MoonlitMixes.ObjectSpwan
{
    public class ItemSpawnZone : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabArray;
        [SerializeField] private Vector2 _spawnMinMax; 
        [SerializeField] private ScriptableObjectSpawnData _objectSpawnScriptableData;

        private Mesh _mesh;
        private Vector3[] _verticesArray;
        private List<Vector3> _usedVerticeList = new List<Vector3>();

        private void Start()
        {
            if(_objectSpawnScriptableData.objectSpawnedList.Count == 0)
            {
                _mesh = GetComponent<MeshCollider>().sharedMesh;
                _verticesArray = _mesh.vertices;

                if(_verticesArray.Length < _spawnMinMax.y)
                {
                    Debug.LogWarning("Too much objects compared to the number of vertices");
                    
                    #if UNITY_EDITOR
                        EditorApplication.ExitPlaymode();
                    #endif
                    return;
                }

                foreach(GameObject obj in _prefabArray)
                {
                    GameObject createdObject = Instantiate(obj);
                    createdObject.transform.position = gameObject.transform.position + SelectRandomVertice();
                }
                
                int nbOfSpawn = (int)Random.Range(_spawnMinMax.x, _spawnMinMax.y);
        
                for (int i = 0; i < nbOfSpawn; i++)
                {
                    int itemSelection = Random.Range(0,_prefabArray.Length);
                    GameObject createdObject = Instantiate(_prefabArray[itemSelection]);
                    createdObject.transform.position = gameObject.transform.position + SelectRandomVertice();

                }
            }
            else
            {
                for (int i = 0; i < _objectSpawnScriptableData.objectSpawnedList.Count; i++)
                {
                    GameObject createdObject = Instantiate(_objectSpawnScriptableData.objectSpawnedList[i]._object);
                    createdObject.transform.position = _objectSpawnScriptableData.objectSpawnedList[i]._verticesSpawnPosition;
                }
            }
        }
        
        private Vector3 SelectRandomVertice()
        {
            bool isVerticeAvailable = false;
            Vector3 vertice = new Vector3();

            while(!isVerticeAvailable)
            {
                vertice = _verticesArray[Random.Range(0, _verticesArray.Length)];
                if(!_usedVerticeList.Contains(vertice))
                {
                    _usedVerticeList.Add(vertice);
                    isVerticeAvailable = true;
                } 
            }

            return vertice;
        }
    }
}