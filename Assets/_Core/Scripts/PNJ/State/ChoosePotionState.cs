using MoonlitMixes.Shop.PotionChoice;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class ChoosePotionState : IPNJState
    {
        private PotionChoiceController _potionChoice;
        private bool _isWaitingForChoice = true;
        private string _potionNameSelect;

        public void EnterState(PNJData data)
        {
            _potionChoice = Object.FindFirstObjectByType<PotionChoiceController>();

            _potionChoice.ShowPotionChoices();

            _isWaitingForChoice = true;
            PotionChoiceController.OnPotionChoiceSelected += OnPotionSelected;
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine)
        {
            if (!_isWaitingForChoice)
            {
                stateMachine.SetSelectedPotion(_potionNameSelect);

                stateMachine.NextState();
            }
        }

        public void ExitState(PNJData data)
        {
            PotionChoiceController.OnPotionChoiceSelected -= OnPotionSelected;
        }

        private void OnPotionSelected(string potionName)
        {
            _potionNameSelect = potionName;
            _isWaitingForChoice = false;
            Debug.Log("Potion choisie: " + potionName);
        }
    }
}