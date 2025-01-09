using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class CuttingBoard : ACookingMachine
    {
        [SerializeField] protected GameObject InteractUI;
        protected bool _isActive = false;
        
        public override void TogleShowInteractivity()
        {
            _isActive = !_isActive;
            InteractUI.SetActive(_isActive);
        }
    }
}