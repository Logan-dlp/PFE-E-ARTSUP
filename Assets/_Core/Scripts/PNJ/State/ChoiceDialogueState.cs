using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.AI.PNJ;
using UnityEngine;
using System.Collections;
using MoonlitMixes.Potion;
using TMPro;

public class ChoiceDialogueState : IPNJState
{
    private PNJData _pnjData;
    private DialogueController _dialogueControllerSuccess;
    private DialogueController _dialogueControllerFailure;
    private DialogueController _dialogueControllerNoPotion;
    private PotionChoiceController _potionChoiceController;
    private PNJStateMachine _pnjStateMachine;
    private PotionPriceCalculate _potionPriceCalculated;

    public void EnterState(PNJData data)
    {
        _pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();
        _potionChoiceController = GameObject.FindObjectOfType<PotionChoiceController>();
        _potionPriceCalculated = GameObject.FindObjectOfType<PotionPriceCalculate>();

        if (_pnjStateMachine != null)
        {
            _dialogueControllerSuccess = _pnjStateMachine.DialogueControllerSuccess;
            _dialogueControllerFailure = _pnjStateMachine.DialogueControllerFailure;
            _dialogueControllerNoPotion = _pnjStateMachine.DialogueControllerNoPotion;
        }

        _pnjData = data;
        _pnjData.Agent.isStopped = true;
        _pnjData.Animator.SetBool("isWalking", false);

        if (_potionChoiceController.SelectedPotionName == _pnjStateMachine.SelectedPotionName)
        {
            _pnjStateMachine.ResetFailedAttempts();
            _dialogueControllerSuccess?.StartDialogue();
            if (_potionPriceCalculated != null)
            {
                int basePrice = 100;
                float multiplier = 1f;

                _potionPriceCalculated.CalculatePotionPrice(basePrice, multiplier);
            }
        }
        else if (string.IsNullOrEmpty(_pnjStateMachine.SelectedPotionName))
        {
            _pnjStateMachine.ResetFailedAttempts();
            _dialogueControllerNoPotion?.StartDialogue();
            if (_potionPriceCalculated != null)
            {
                int basePrice = 100;
                float multiplier = 0f;

                _potionPriceCalculated.CalculatePotionPrice(basePrice, multiplier);
            }
        }
        else
        {
            _pnjStateMachine.IncrementFailedAttempts();
            _dialogueControllerFailure?.StartDialogue();
            if (_potionPriceCalculated != null)
            {
                int basePrice = 100;
                float multiplier = 0.5f;

                _potionPriceCalculated.CalculatePotionPrice(basePrice, multiplier);
            }
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
            else if (_pnjStateMachine.FailedAttempts == 0 || _pnjStateMachine.FailedAttempts >= 3)
            {
                _pnjStateMachine.NextState();
            }
        }
        _dialogueControllerFailure.EndDialogue();
        DialogueController.OnDialogueFinished -= OnDialogueEnd;
    }

    public void UpdateState(PNJData data, PNJStateMachine stateMachine) { }

    public void ExitState(PNJData data)
    {
        data.Agent.isStopped = false;
    }
}