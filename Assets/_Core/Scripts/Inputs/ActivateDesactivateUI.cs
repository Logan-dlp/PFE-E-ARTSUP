using MoonlitMixes.Inputs;
using UnityEngine;

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
