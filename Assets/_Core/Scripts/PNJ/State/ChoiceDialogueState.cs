using UnityEngine;
using MoonlitMixes.Potion;
using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.AI.PNJ;

public class ChoiceDialogueState : IPNJState
{
    private PNJData _pnjData;
    private DialogueController _dialogueControllerSuccess;
    private DialogueController _dialogueControllerFailure;
    private DialogueController _dialogueControllerNoPotion;
    private PotionChoiceController _potionChoiceController;
    private PNJStateMachine _pnjStateMachine;
    private PotionPriceCalculate _potionPriceCalculated;
    private PotionInventory _potionInventory;

    public void EnterState(PNJData data)
    {
        _pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();
        _potionChoiceController = GameObject.FindObjectOfType<PotionChoiceController>();
        _potionPriceCalculated = GameObject.FindObjectOfType<PotionPriceCalculate>();
        _potionInventory = GameObject.FindObjectOfType<PotionInventory>();

        if (_pnjStateMachine != null)
        {
            _dialogueControllerSuccess = _pnjStateMachine.DialogueControllerSuccess;
            _dialogueControllerFailure = _pnjStateMachine.DialogueControllerFailure;
            _dialogueControllerNoPotion = _pnjStateMachine.DialogueControllerNoPotion;
        }

        _pnjData = data;
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

        if (_potionChoiceController.SelectedPotionName == _pnjStateMachine.SelectedPotionName)
        {
            _dialogueControllerSuccess?.StartDialogue();
            _potionPriceCalculated?.CalculatePotionPrice(potionPrice, _pnjStateMachine.FailedAttempts);
        }
        else if (string.IsNullOrEmpty(_pnjStateMachine.SelectedPotionName))
        {
            _pnjStateMachine.ResetFailedAttempts();
            _dialogueControllerNoPotion?.StartDialogue();
            _potionPriceCalculated?.CalculatePotionPrice(0, 0);
        }
        else
        {
            _pnjStateMachine.IncrementFailedAttempts();
            _dialogueControllerFailure?.StartDialogue();
            _potionPriceCalculated?.CalculatePotionPrice(potionPrice, _pnjStateMachine.FailedAttempts);
        }

        DialogueController.OnDialogueFinished += OnDialogueEnd;
    }

    private void OnDialogueEnd()
    {
        DialogueController.OnDialogueFinished -= OnDialogueEnd;

        if (_potionChoiceController.SelectedPotionName == _pnjStateMachine.SelectedPotionName || string.IsNullOrEmpty(_pnjStateMachine.SelectedPotionName))
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
        _dialogueControllerFailure.EndDialogue();
    }

    public void UpdateState(PNJData data, PNJStateMachine stateMachine) { }

    public void ExitState(PNJData data)
    {
        data.Agent.isStopped = false;
    }
}