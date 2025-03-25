using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class SceneStarter : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private void Start()
        {
            SceneLoader.InitSceneLoader();
        }

        public void ChangeScene(string sceneName)
        {
            SceneLoader.LoadAsyncScene(sceneName, _animator);
        }
    }
}
