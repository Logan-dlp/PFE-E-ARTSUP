using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.UI
{
    public class OpenRecipeCanvas : MonoBehaviour
    {
        public void OpenCloseCanvasWithInput(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                gameObject.SetActive(!gameObject.activeSelf);
                
                if (gameObject.activeSelf)
                {
                    InputManager.Instance.SwitchActionMap("Recipe");
                }
                else
                {
                    InputManager.Instance.SwitchActionMap("Player");
                }
            }
        }
    }
}