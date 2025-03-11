using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class ChoosePotionState : IPNJState
    {
        private PotionChoiceController _potionChoice;
        private bool _isWaitingForChoice = true;

        public void EnterState(PNJData data)
        {
            _potionChoice = GameObject.FindObjectOfType<PotionChoiceController>();

            _potionChoice.ShowPotionChoices();

            PotionChoiceController.OnPotionChoiceSelected += OnPotionSelected;
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine)
        {
            if (!_isWaitingForChoice)
            {
                stateMachine.NextState();
            }
        }

        public void ExitState(PNJData data)
        {
            PotionChoiceController.OnPotionChoiceSelected -= OnPotionSelected;
        }

        private void OnPotionSelected(string potionName)
        {
            _isWaitingForChoice = false;
            Debug.Log("Potion choisie: " + potionName);
        }
    }
}