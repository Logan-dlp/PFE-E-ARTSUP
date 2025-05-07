using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using MoonlitMixes.Scene;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.UI
{
    public class ChangeSceneUI : MonoBehaviour, IUIActivationControl
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _panel;
        [SerializeField] private LastSceneNameData _lastSceneNameData;
        private string _sceneName;

        public Animator AnimatorUI => _animator;
        public GameObject Panel => _panel;

        public void OpenCanvas(string sceneName)
        {
            _panel.SetActive(true);
            _sceneName = sceneName;
        }

        public void OpenCanvas()
        {
            
        }

        public void CloseCanvas()
        {
            _panel.SetActive(false);
            InputManager.Instance.SwitchActionMap("Player");
        }

        public void ChangeScene(InputAction.CallbackContext callbackContext)
        {
            if(callbackContext.started)
            {
                _lastSceneNameData.sceneName = SceneManager.GetActiveScene().name;
                SceneLoader.LoadAsyncScene(_sceneName, _animator);
            }
        }
    }
}
