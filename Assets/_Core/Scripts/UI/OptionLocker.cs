using UnityEngine;

namespace MoonlitMixes.UI
{
    public class OptionLocker : MonoBehaviour
    {
        private void OnEnable()
        {
            OptionPageUI.CanCloseOptions = true;
        }

        private void OnDisable()
        {
            OptionPageUI.CanCloseOptions = false;
        }
    }
}
