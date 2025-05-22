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

        private static bool _isLoading = false;

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
            _panelNoChestItem.SetActive(false);
            _hasPopup = false;
            InputManager.Instance.SwitchActionMap("Player");

            if (_sceneName == "S_Forest")
                Debug.Log("_panelNoChestItem.SetActive(false)");
        }

        public void ChangeScene(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started && !_isLoading)
            {
                Debug.Log("ChangeScene triggered for: " + _sceneName);

                if (!_hasPopup)
                {
                    _lastSceneNameData.sceneName = SceneManager.GetActiveScene().name;

                    if (_sceneName == _sceneTransfereItem)
                    {
                        if (TryGetComponent(out SendItemExit sendItemExit))
                        {
                            if (sendItemExit.SendItems())
                            {
                                Debug.Log("Items sent successfully, loading scene...");
                                _isLoading = true;
                                SceneLoader.LoadAsyncScene(_sceneName, _animator);
                            }
                            else
                            {
                                Debug.Log("No items to send, showing popup.");
                                _hasPopup = true;
                                _panelNoChestItem.SetActive(true);
                                _panel.SetActive(false);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("SendItemExit component not found.");
                        }
                    }
                    else
                    {
                        Debug.Log("Scene does not require item transfer, loading scene...");
                        _isLoading = true;
                        SceneLoader.LoadAsyncScene(_sceneName, _animator);
                    }
                }
                else
                {
                    Debug.Log("Popup already open, forcing scene change...");
                    ConfirmForceChangeScene();
                }
            }
        }

        public void ConfirmForceChangeScene()
        {
            Debug.Log("ConfirmForceChangeScene called.");
            _panelNoChestItem.SetActive(false);
            _hasPopup = false;
            _isLoading = false;
            ForceChangeScene();
        }

        private void ForceChangeScene()
        {
            if (_isLoading)
            {
                Debug.LogWarning("Scene already loading, skipping ForceChangeScene.");
                return;
            }

            Debug.Log("ForceChangeScene: Loading " + _sceneName);
            _isLoading = true;
            if (string.IsNullOrEmpty(_sceneName))
            {
                Debug.LogError("No scene name set to load!");
                _isLoading = false;
                return;
            }
            SceneLoader.LoadAsyncScene(_sceneName, _animator);
        }
    }
}