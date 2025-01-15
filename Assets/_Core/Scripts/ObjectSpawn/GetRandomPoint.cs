using UnityEngine;

namespace MoonlitMixes.ObjectSpwan
{
    public class GetRandomPoint : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabArray;
        [SerializeField] private Vector2 _spawnMinMax; 
    
        private Mesh _mesh;
        private Vector3[] _verticesArray;
    
        private void Start()
        {
            _mesh = GetComponent<MeshCollider>().sharedMesh;
            _verticesArray = _mesh.vertices;

            int nbOfSpawn = (int)Random.Range(_spawnMinMax.x, _spawnMinMax.y);
        
            for (int i = 0; i < nbOfSpawn; i++)
            {
                GameObject createdObject = Instantiate(_prefabArray[Random.Range(0,_prefabArray.Length)]);
                createdObject.transform.position = gameObject.transform.position + _verticesArray[Random.Range(0, _verticesArray.Length)];
            }
        }
    }
}