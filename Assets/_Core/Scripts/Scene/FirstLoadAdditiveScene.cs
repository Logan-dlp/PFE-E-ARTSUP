using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.Scene
{
    public class FirstLoadAdditiveScene : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            SceneManager.LoadScene("S_TimeIndincation", LoadSceneMode.Additive);
            SceneManager.LoadScene("S_Menu_UI", LoadSceneMode.Additive);
        }
    }
}
