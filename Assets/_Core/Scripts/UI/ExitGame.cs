using UnityEditor;
using UnityEngine;

namespace MoonlitMixes.UI
{
    public class ExitGame : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}
