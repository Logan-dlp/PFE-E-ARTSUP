using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class JoinScene : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoad;

        public void ChangeScene()
        {
            FindAnyObjectByType<SceneStarter>().ChangeScene(_sceneToLoad);
        }
    }
}