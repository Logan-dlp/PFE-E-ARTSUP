using MoonlitMixes.Dialogue;
using MoonlitMixes.Potion;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class ChoosePotionState : IPNJState
    {
        public static event System.Action OnPotionSelectedSoundRequested;

        private PotionChoiceController _potionChoice;
        private bool _isWaitingForChoice = true;
        private PotionResult _potionResultSelected;

        public void EnterState(PNJData data)
        {
            _potionChoice = Object.FindFirstObjectByType<PotionChoiceController>();

            if (_potionChoice != null)
            {
                _potionChoice.ShowPotionChoices(data.CurrentPotionIndex);

                _isWaitingForChoice = true;
                PotionChoiceController.OnPotionChoiceSelected += OnPotionSelected;
            }
            else
            {
                Debug.LogWarning("[ChoosePotionState] Aucun PotionChoiceController trouvé.");
                _isWaitingForChoice = false;
            }
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (!_isWaitingForChoice)
            {
                data.OnPotionSelected?.Invoke(_potionResultSelected);
                return new ChoiceDialogueState();
            }

            return null;
        }

        public void ExitState(PNJData data)
        {
            PotionChoiceController.OnPotionChoiceSelected -= OnPotionSelected;
        }

        private void OnPotionSelected(PotionResult potionResult)
        {
            _potionResultSelected = potionResult;
            _isWaitingForChoice = false;

            Debug.Log($"[ChoosePotionState] Potion choisie par le joueur : {potionResult?.Recipe?.RecipeName ?? "null"}");

            OnPotionSelectedSoundRequested?.Invoke();
        }
    }
}