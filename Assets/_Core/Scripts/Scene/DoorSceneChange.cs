using MoonlitMixes.Inputs;
using MoonlitMixes.UI;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class DoorSceneChange : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public string SceneName
        {
            get => _sceneName;
        }

        public void OpenCanvas()
        {
            var canvasUI = FindFirstObjectByType<ChangeSceneUI>();
            if (canvasUI != null)
            {
                canvasUI.OpenCanvas(_sceneName);
                InputManager.Instance.SwitchActionMap("ChangeScene");
            }
            else
            {
                Debug.LogError("ChangeSceneUI est introuvable dans la scène !");
            }
        }
    }
}
