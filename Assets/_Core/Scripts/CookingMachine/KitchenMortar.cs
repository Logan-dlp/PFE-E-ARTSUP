using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class KitchenMortar : ACookingMachine
    {
        [SerializeField] private GameObject InteractUI;
        private bool _isActive = false;
        
        public override void TogleShowInteractivity()
        {
            _isActive = !_isActive;
            InteractUI.SetActive(_isActive);
        }
    }
}