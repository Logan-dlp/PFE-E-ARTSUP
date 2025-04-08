using MoonlitMixes.Inputs;
using MoonlitMixes.Scene;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeSceneUI : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _panel;
    private string _sceneName;
    
    public void OpenCanvas(string sceneName)
    {
        _panel.SetActive(true);
        _sceneName = sceneName;
    }

    public void ChangeScene(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.started)
        {
            SceneLoader.LoadAsyncScene(_sceneName, _animator);
        }
    }

    public void CloseCanvas()
    {
        _panel.SetActive(false);
        InputManager.Instance.SwitchActionMap("Player");
    }
}
