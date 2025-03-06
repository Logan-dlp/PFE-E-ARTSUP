using UnityEngine;
using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using static MoonlitMixes.Datas.ScriptableObjectSpawnData;

namespace MoonlitMixes.Item
{
    public class ObjectSave : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent scriptableEvent;
        [SerializeField] private ScriptableObjectSpawnData _scriptableObjectSpawnData;
        PrefabHolder[] allInteractableObjectArray;
        
        private void OnEnable()
        {
            scriptableEvent.OnEvent += Save;
        }
        private void OnDisable()
        {
            scriptableEvent.OnEvent -= Save;
        }

        private void Save()
        {
            allInteractableObjectArray = FindObjectsByType<PrefabHolder>(FindObjectsSortMode.None);
            foreach (PrefabHolder obj in allInteractableObjectArray)
            {
                _scriptableObjectSpawnData.objectSpawnedList.Add(new ObjectSpawned(obj.prefab, obj.transform.position));
            }
        }
    }
}
