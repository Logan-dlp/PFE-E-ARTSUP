using UnityEngine;
using MoonlitMixes.ExplorationTools;

namespace MoonlitMixes.UI
{
    public class InteractionUITrigger : AInteractionUI
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 10)
            {
                if (_toolType == ToolType.Hand || _toolType == ToolType.Staff)
                {
                    ActivationInteractionUI(true);
                }
                else
                {
                    if (other.GetComponent<UseTools>().CurrentTool != _toolType) return;

                    ActivationInteractionUI(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 10)
            {
                ActivationInteractionUI(false);
            }
        }
    }
}
