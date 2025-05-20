using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace MoonlitMixes.SaveSystems
{
    using Singleton;
    using Extensions;
    
    public class SaveSystem : PersistentMonoSingleton<SaveSystem>
    {
        private List<GameObject> _currentSerializableGameObjectList = new();
        
        protected override void Awake()
        {
            base.Awake();
            
            foreach (GameObject gObj in gameObject.FindGameObjectsOfType<ISerializable>())
            {
                _currentSerializableGameObjectList.Add(gObj);
            }
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            });
        }
        
        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
        
        public void SaveAllScene()
        {
            for (int i = 0; i < _currentSerializableGameObjectList.Count; ++i)
            {
                string currentFilePath = $"{Application.persistentDataPath}/{_currentSerializableGameObjectList[i].GetInstanceID()}.json";
                try
                {
                    string json = _currentSerializableGameObjectList[i].GetComponent<ISerializable>().Serialize();
                    
                    Debug.Log(json);
                    Debug.Log($"{Application.persistentDataPath}/{_currentSerializableGameObjectList[i].GetInstanceID()}.json");
                    
                    using FileStream stream = new(currentFilePath, FileMode.Create);
                    using StreamWriter writer = new(stream);
                    writer.Write(json);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"{_currentSerializableGameObjectList[i].GetInstanceID()} - {ex}");
                }
            }
        }
        
        public void LoadAllScene()
        {
            GameObject[] allGameObjectSerializable = gameObject.FindGameObjectsOfType<ISerializable>();
            for (int i = 0; i < allGameObjectSerializable.Length; ++i)
            {
                string currentFilePath = $"{Application.persistentDataPath}/{allGameObjectSerializable[i].GetInstanceID()}.json";
                if (File.Exists(currentFilePath))
                {
                    try
                    {
                        using StreamReader reader = new(currentFilePath);
                        string json = reader.ReadToEnd();
                        
                        allGameObjectSerializable[i].GetComponent<ISerializable>().Deserialize(json);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"{allGameObjectSerializable[i].GetInstanceID()}: {ex.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"{allGameObjectSerializable[i].name} have not been loaded !");
                }
            }
        }
    }
}