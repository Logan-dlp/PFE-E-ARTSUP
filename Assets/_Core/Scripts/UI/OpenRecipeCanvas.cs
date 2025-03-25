using UnityEngine;

namespace MoonlitMixes.UI
{
    public class OpenRecipeCanvas : MonoBehaviour
    {
        public void OpenCloseCanvas()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}