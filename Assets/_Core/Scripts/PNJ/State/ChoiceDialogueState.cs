using MoonlitMixes.Dialogue;
using MoonlitMixes.Potion;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class ChoiceDialogueState : IPNJState
    {
        private PNJData _pnjData;
        private PNJStateMachine _pnjStateMachine;
        private PotionInventory _potionInventory;
        private PotionPriceCalculate _potionPriceCalculated;
        private PotionChoiceController _potionChoiceController;

        public void EnterState(PNJData data)
        {
            _pnjData = data;
            _pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();
            _potionInventory = Object.FindFirstObjectByType<PotionInventory>();
            _potionPriceCalculated = Object.FindFirstObjectByType<PotionPriceCalculate>();
            _potionChoiceController = Object.FindFirstObjectByType<PotionChoiceController>();

            _pnjData.Agent.isStopped = true;
            _pnjData.Animator.SetBool("isWalking", false);

            int potionPrice = 100;
            if (_potionInventory != null)
            {
                PotionResult selectedPotion = _potionInventory.PotionList.Find(p => p.Recipe.RecipeName == _pnjStateMachine.SelectedPotionName);
                if (selectedPotion != null)
                {
                    potionPrice = selectedPotion.Price;
                }
            }

            DialogueController.OnDialogueFinished += OnDialogueEnd;

            if (_potionChoiceController.SelectedPotionName == _pnjStateMachine.SelectedPotionName)
            {
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, _pnjStateMachine.FailedAttempts);
                DialogueController.Instance.StartDialogue(_pnjStateMachine.SuccessDialogueData);
            }
            else if (string.IsNullOrEmpty(_pnjStateMachine.SelectedPotionName))
            {
                _pnjStateMachine.ResetFailedAttempts();
                _potionPriceCalculated?.CalculatePotionPrice(0, 0);
                DialogueController.Instance.StartDialogue(_pnjStateMachine.NoPotionDialogueData);
            }
            else
            {
                _pnjStateMachine.IncrementFailedAttempts();
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, _pnjStateMachine.FailedAttempts);
                DialogueController.Instance.StartDialogue(_pnjStateMachine.FailureDialogueData);
            }
        }

        private void OnDialogueEnd()
        {
            DialogueController.OnDialogueFinished -= OnDialogueEnd;

            if (_potionChoiceController.SelectedPotionName == _pnjStateMachine.SelectedPotionName ||
                string.IsNullOrEmpty(_pnjStateMachine.SelectedPotionName))
            {
                _pnjStateMachine.ResetFailedAttempts();
                _pnjStateMachine.NextState();
            }
            else
            {
                if (_pnjStateMachine.FailedAttempts < 3)
                {
                    _pnjStateMachine.TransitionToState(3);
                }
                else
                {
                    _pnjStateMachine.NextState();
                }
            }
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine) { }

        public void ExitState(PNJData data)
        {
            data.Agent.isStopped = false;
        }
    }
}