using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class SceneStarter : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void ChangeScene(string sceneName)
        {
            SceneLoader.LoadAsyncScene(sceneName, _animator);
        }
    }
}
