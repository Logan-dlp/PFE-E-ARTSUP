using MoonlitMixes.Player;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class CauldronMixing : ACookingMachine
    {
        [SerializeField] private GameObject _interactUI;
        private bool _isActive = false;
        private CauldronRecipeChecker _cauldronRecipeChecker;


        private void Awake()
        {
            _cauldronRecipeChecker = GetComponent<CauldronRecipeChecker>();
        }
        public override void TogleShowInteractivity()
        {
            _isActive = !_isActive;
            //_interactUI.SetActive(_isActive);
        }

        public override void SuccesItem()
        {
            _cauldronRecipeChecker.CheckQTE(true);
            Desactivate();
        }

        public override void FailedItem()
        {
            Desactivate();
        }

        public void ConvertItem(PlayerInteraction player)
        {
            Activate();
            _scriptableQTEConfig.ScriptableBoolEvent = _scriptableBoolEvent;
            _scriptableQTEConfig.QteSlot = _imageQTE;
            _scriptableQTEConfig.ProgressBarUI = _imageProgressBar;
            _scriptableQTEEvent.SendObject(_scriptableQTEConfig);
            _playerInteraction = player;
        }
    }
}
