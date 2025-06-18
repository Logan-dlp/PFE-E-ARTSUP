using MoonlitMixes.Inputs;
using UnityEngine;

namespace MoonlitMixes.UI
{
    public class ActivateDesactivateUI : MonoBehaviour
    {
        private void OnEnable()
        {
            InputManager.Instance.SwitchActionMap("UI");
        }

        private void OnDisable()
        {
            InputManager.Instance.SwitchActionMap("Player");
        }
    }
}
