using MoonlitMixes.Inputs;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class DoorSceneChange : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private string _sceneName;

        public void OpenCanvas()
        {
            FindFirstObjectByType<ChangeSceneUI>().OpenCanvas(_sceneName);
            InputManager.Instance.SwitchActionMap("UI");
        }
    }
}
