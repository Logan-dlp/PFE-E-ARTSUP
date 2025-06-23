using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Scene
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _creditsUi;
        [SerializeField] private Button _creditsUiButton;
        [SerializeField] private Button _creditsMenuUiButton;

        public void OpenCredits()
        {
            _creditsUi.SetActive(true);
            _creditsUiButton.Select();
        }

        public void CloseCredits()
        {
            _creditsUi.SetActive(false);
            _creditsMenuUiButton.Select();
        }

        public void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}