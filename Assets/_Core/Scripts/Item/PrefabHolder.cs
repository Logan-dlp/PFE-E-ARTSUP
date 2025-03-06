using UnityEngine;

namespace MoonlitMixes.Item
{
    public class PrefabHolder : MonoBehaviour
    {
        public GameObject prefab;

        private void Start()
        {
            prefab = (GameObject)Resources.Load(name.Remove(name.Length - 7));
        }
    }
}
