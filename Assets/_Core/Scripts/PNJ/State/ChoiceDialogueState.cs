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
        private int _failedAttempt = 0;

        public void EnterState(PNJData data)
        {
            _potionInventory = Object.FindFirstObjectByType<PotionInventory>();
            _potionPriceCalculated = Object.FindFirstObjectByType<PotionPriceCalculate>();
            _potionChoiceController = Object.FindFirstObjectByType<PotionChoiceController>();

            data.agent.isStopped = true;
            data.animator.SetBool("isWalking", false);

            int potionPrice = 100;
            if (_potionInventory != null)
            {
                PotionResult selectedPotion = _potionInventory.PotionList.Find(p => p == data.selectedPotionResult);
                if (selectedPotion != null)
                {
                    potionPrice = selectedPotion.Price;
                }
            }

            DialogueController.OnDialogueFinished += OnDialogueEnd;

            if (IsSelectedPotionValid(data))
            {
                _isSuccess = true;
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, _failedAttempt);
                DialogueController.Instance.StartDialogue(data.successDialogueData);
            }
            //else if (/*IsNullOrEmpty = si c'est 0 valid*/data.selectedPotionResult)
            //{
            //    _isNoPotion = true;
            //    ResetFailedAttempts();
            //    _potionPriceCalculated?.CalculatePotionPrice(0, 0);
            //    DialogueController.Instance.StartDialogue(data.noPotionDialogueData);
            //}
            else
            {
                IncrementFailedAttempts();
                _potionPriceCalculated?.CalculatePotionPrice(potionPrice, _failedAttempt);
                DialogueController.Instance.StartDialogue(data.failureDialogueData);
            }
        }

        private bool IsSelectedPotionValid(PNJData data)
        {
            return System.Array.Exists(data.requestPotionArray, p => p == data.selectedPotionResult);
        }


        private void IncrementFailedAttempts()
        {

            _failedAttempt++;
        }

        private void ResetFailedAttempts()
        {
            _failedAttempt = 0;
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (_isDialogueFinished)
            {
                DialogueController.OnDialogueFinished -= OnDialogueEnd;

                if (_isSuccess || _isNoPotion)
                {
                    ResetFailedAttempts();
                    return new MoveToStartState();
                }
                else
                {
                    if (_failedAttempt < 3)
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
            data.agent.isStopped = false;
            DialogueController.OnDialogueFinished -= OnDialogueEnd;
        }

        private void OnDialogueEnd()
        {
            _isDialogueFinished = true;
        }
    }
}