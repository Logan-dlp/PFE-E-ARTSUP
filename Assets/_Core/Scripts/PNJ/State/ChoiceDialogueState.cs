using MoonlitMixes.Dialogue;
using MoonlitMixes.Potion;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class ChoiceDialogueState : IPNJState
    {
        private PotionInventory _potionInventory;
        private PotionPriceCalculate _potionPriceCalculated;
        private PotionChoiceController _potionChoiceController;
        private bool _isDialogueFinished = false;
        private bool _isSuccess = false;
        private bool _isNoPotion = false;

        public void EnterState(PNJData data)
        {
            _potionInventory = Object.FindFirstObjectByType<PotionInventory>();
            _potionPriceCalculated = Object.FindFirstObjectByType<PotionPriceCalculate>();
            _potionChoiceController = Object.FindFirstObjectByType<PotionChoiceController>();

            data.Agent.isStopped = true;
            data.Animator.SetBool("isWalking", false);

            int potionPrice = 100;
            if (_potionInventory != null)
            {
                PotionResult selectedPotion = _potionInventory.PotionList.Find(p => p.Recipe.RecipeName == data.StateMachine.SelectedPotionName);
                if (selectedPotion != null)
                {
                    potionPrice = selectedPotion.Price;
                }
            }

            DialogueController.OnDialogueFinished += OnDialogueEnd;

            if (_potionChoiceController.SelectedPotionName == data.StateMachine.SelectedPotionName)
            {
                _isSuccess = true;
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, data.StateMachine.FailedAttempts);
                DialogueController.Instance.StartDialogue(data.StateMachine.SuccessDialogueData);
            }
            else if (string.IsNullOrEmpty(data.StateMachine.SelectedPotionName))
            {
                _isNoPotion = true;
                data.StateMachine.ResetFailedAttempts();
                _potionPriceCalculated?.CalculatePotionPrice(0, 0);
                DialogueController.Instance.StartDialogue(data.StateMachine.NoPotionDialogueData);
            }
            else
            {
                data.StateMachine.IncrementFailedAttempts();
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, data.StateMachine.FailedAttempts);
                DialogueController.Instance.StartDialogue(data.StateMachine.FailureDialogueData);
            }
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (_isDialogueFinished)
            {
                DialogueController.OnDialogueFinished -= OnDialogueEnd;

                if (_isSuccess || _isNoPotion)
                {
                    data.StateMachine.ResetFailedAttempts();
                    return new MoveToStartState();
                }
                else
                {
                    if (data.StateMachine.FailedAttempts < 3)
                    {
                        return new ChoosePotionState();
                    }
                    else
                    {
                        return new MoveToStartState();
                    }
                }
            }

            return null;
        }

        public void ExitState(PNJData data)
        {
            data.Agent.isStopped = false;
            DialogueController.OnDialogueFinished -= OnDialogueEnd;
        }

        private void OnDialogueEnd()
        {
            _isDialogueFinished = true;
        }
    }
}