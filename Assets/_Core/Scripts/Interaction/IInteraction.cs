using UnityEngine;

namespace MoonlitMixes.Interactions
{
    using ExplorationTools;
    
    public interface IInteraction
    {
        public ToolType GetToolType();
        public void EnableUI();
        public void DisableUI();
        public GameObject Interact();
    }
}