using System;
using UnityEngine;

namespace MoonlitMixes.Singleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                        _instance.OnMonoSingletonCreated();
                    }
                }
                return _instance;
            }
        }
        
        private SingletonInitializationStatus _initializationStatus = SingletonInitializationStatus.None;
        public virtual bool IsInitialized => this._initializationStatus == SingletonInitializationStatus.Initialized;

        public static void CreateInstance()
        {
            DestroyInstance();
            _instance = Instance;
        }

        public static void DestroyInstance()
        {
            if (_instance == null)
                return;
            
            _instance.DestroySingleton();
            _instance = default(T);
        }
        
        public void InitializeSingleton()
        {
            if (_initializationStatus != SingletonInitializationStatus.None)
                return;

            _initializationStatus = SingletonInitializationStatus.Initializing;
            OnInitializing();
            
            _initializationStatus = SingletonInitializationStatus.Initialized;
            OnInitialized();
        }

        public void DestroySingleton()
        {
            
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                InitializeSingleton();
                
                return;
            }

            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
        }

        protected virtual void OnMonoSingletonCreated()
        {
            Debug.Log($"MonoSingleton Created in {gameObject.name} !");
        }

        protected virtual void OnInitializing()
        {
            Debug.Log($"OnInitializing {gameObject.name} !");
        }

        protected virtual void OnInitialized()
        {
            Debug.Log($"OnInitialized {gameObject.name} !");
        }
    }
}