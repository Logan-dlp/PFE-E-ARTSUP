using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using MoonlitMixes.Item;
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
        [SerializeField] private GameObject _panelNoChestItem;
        [SerializeField] private LastSceneNameData _lastSceneNameData;
        [SerializeField] private string _sceneTransfereItem;

        private bool _hasPopup;
        private string _sceneName;
        private InputActionMap inputActions;

        private static bool _isLoading = false;  // Variable statique pour blocage double chargement

        public Animator AnimatorUI => _animator;
        public GameObject Panel => _panel;

        public void OpenCanvas(string sceneName)
        {
            _panel.SetActive(true);
            _sceneName = sceneName;
        }

        public void OpenCanvas() { }

        public void CloseCanvas()
        {
            _panel.SetActive(false);
            InputManager.Instance.SwitchActionMap("Player");
            if (_sceneName == "S_Forest") Debug.Log("_panelNoChestItem.SetActive(false)");
        }

        public void ChangeScene(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                //if (_isLoading)
                //{
                //    Debug.LogWarning("Une scène est déjà en cours de chargement.");
                //    return; 
                //}

                if (!_hasPopup)
                {
                    _lastSceneNameData.sceneName = SceneManager.GetActiveScene().name;

                    if (_sceneName == _sceneTransfereItem)
                    {
                        TryGetComponent(out SendItemExit sendItemExit);
                        if (sendItemExit.SendItems())
                        {
                            _isLoading = true;
                            SceneLoader.LoadAsyncScene(_sceneName, _animator);
                        }
                        else
                        {
                            _hasPopup = true;
                            _panelNoChestItem.SetActive(true);
                            _panel.SetActive(false);
                        }
                    }
                    else
                    {
                        _isLoading = true;
                        SceneLoader.LoadAsyncScene(_sceneName, _animator);
                    }
                }
                else
                {
                    ForceChangeScene();
                }
            }
        }

        private void ForceChangeScene()
        {
            //if (_isLoading)
            //{
            //    Debug.LogWarning("Une scène est déjà en cours de chargement.");
            //    return;
            //}

            _isLoading = true;
            SceneLoader.LoadAsyncScene(_sceneName, _animator);
        }

        // Méthode à appeler à la fin du chargement pour réinitialiser le flag
        public static void OnSceneLoadComplete()
        {
            _isLoading = false;
        }
    }
}