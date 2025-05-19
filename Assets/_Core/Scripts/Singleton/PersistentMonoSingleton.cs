using UnityEngine;

namespace MoonlitMixes.Singleton
{
    public abstract class PersistentMonoSingleton<T> : MonoSingleton<T> where T : MonoSingleton<T>
    {
        [SerializeField] private bool _unparentOnAwake = true;

        protected override void OnInitializing()
        {
            if (_unparentOnAwake)
                transform.SetParent(null);
            
            base.OnInitializing();

            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }
    }
}