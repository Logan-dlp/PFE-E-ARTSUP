using UnityEngine;
using MoonlitMixes.Potion;
using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.AI.PNJ;
using MoonlitMixes.Dialogue;

public class ChoiceDialogueState : IPNJState
{
    private PNJData _pnjData;
    private PotionChoiceController _potionChoiceController;
    private PNJStateMachine _pnjStateMachine;
    private PotionPriceCalculate _potionPriceCalculated;
    private PotionInventory _potionInventory;

    public void EnterState(PNJData data)
    {
        _pnjData = data;
        _pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();
        _potionChoiceController = Object.FindFirstObjectByType<PotionChoiceController>();
        _potionPriceCalculated = Object.FindFirstObjectByType<PotionPriceCalculate>();
        _potionInventory = Object.FindFirstObjectByType<PotionInventory>();

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
            DialogueController.Instance.StartDialogue(_pnjStateMachine._successDialogueData);
        }
        else if (string.IsNullOrEmpty(_pnjStateMachine.SelectedPotionName))
        {
            _pnjStateMachine.ResetFailedAttempts();
            _potionPriceCalculated?.CalculatePotionPrice(0, 0);
            DialogueController.Instance.StartDialogue(_pnjStateMachine._noPotionDialogueData);
        }
        else
        {
            _pnjStateMachine.IncrementFailedAttempts();
            _potionPriceCalculated?.CalculatePotionPrice(potionPrice, _pnjStateMachine.FailedAttempts);
            DialogueController.Instance.StartDialogue(_pnjStateMachine._failureDialogueData);
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