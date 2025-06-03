using MoonlitMixes.Datas;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoonlitMixes.Scene
{
    public class SceneStarter : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _transitionBG;
        [SerializeField] private Image _transitionGIF;
        [SerializeField] private LastSceneNameData _lastSceneNameData;

        public void ChangeScene(string sceneName)
        {
            _lastSceneNameData.sceneName = SceneManager.GetActiveScene().name;
            SceneLoader.LoadAsyncScene(sceneName, _animator);

        }

        private void Update()
        {
            _transitionGIF.color = new Color(_transitionGIF.color.r, _transitionGIF.color.g, _transitionGIF.color.b, _transitionBG.color.a);
        }
    }
}