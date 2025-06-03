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
            if (_potionInventory != null && data.SelectedPotionResult != null)
            {
                PotionResult selectedPotion = _potionInventory.PotionList.Find(p => p == data.SelectedPotionResult);
                if (selectedPotion != null)
                {
                    potionPrice = selectedPotion.Price;
                }
            }

            DialogueController.OnDialogueFinished += OnDialogueEnd;

            if (data.SelectedPotionResult == null)
            {
                _isNoPotion = true;
                data.FailedAttempt = 0; // Reset if no potion selected
                _potionPriceCalculated?.CalculatePotionPrice(0, 0);
                DialogueController.Instance.StartDialogue(data.NoPotionDialogueData);
            }
            else if (IsSelectedPotionValid(data))
            {
                _isSuccess = true;
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, data.FailedAttempt);
                DialogueController.Instance.StartDialogue(data.SuccessDialogueData);
            }
            else
            {
                data.FailedAttempt++;
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, data.FailedAttempt);
                DialogueController.Instance.StartDialogue(data.FailureDialogueData);
            }
        }

        private bool IsSelectedPotionValid(PNJData data)
        {
            return data.SelectedPotionResult != null &&
                   data.CurrentPotionIndex < data.RequestPotionArray.Length &&
                   data.SelectedPotionResult == data.RequestPotionArray[data.CurrentPotionIndex];
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (_isDialogueFinished)
            {
                DialogueController.OnDialogueFinished -= OnDialogueEnd;

                if (_isSuccess)
                {
                    data.PotionValidList.Add(data.SelectedPotionResult);
                    data.CurrentPotionIndex++;
                    data.FailedAttempt = 0; // Reset on success

                    if (data.PotionValidList.Count < data.RequestPotionArray.Length)
                        return new SecondDialogueState();
                    else
                        return new MoveToStartState();
                }

                if (_isNoPotion || data.FailedAttempt >= 3)
                    return new MoveToStartState();

                return new ChoosePotionState(); // Retry if not max failed
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