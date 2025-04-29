using MoonlitMixes.Dialogue;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class ChoosePotionState : IPNJState
    {
        private PotionChoiceController _potionChoice;
        private bool _isWaitingForChoice = true;
        private string _potionNameSelected;

        public void EnterState(PNJData data)
        {
            _potionChoice = Object.FindFirstObjectByType<PotionChoiceController>();

            if (_potionChoice != null)
            {
                _potionChoice.ShowPotionChoices();
                _isWaitingForChoice = true;
                PotionChoiceController.OnPotionChoiceSelected += OnPotionSelected;
            }
            else
            {
                _isWaitingForChoice = false;
            }
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (!_isWaitingForChoice)
            {
                data.StateMachine.SetSelectedPotion(_potionNameSelected);

                return new ChoiceDialogueState();
            }

            return null; 
        }

        public void ExitState(PNJData data)
        {
            PotionChoiceController.OnPotionChoiceSelected -= OnPotionSelected;
        }

        private void OnPotionSelected(string potionName)
        {
            _potionNameSelected = potionName;
            _isWaitingForChoice = false;
            Debug.Log("Potion choisie: " + potionName);
        }
    }
}
