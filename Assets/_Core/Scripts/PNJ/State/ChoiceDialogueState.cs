using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.AI.PNJ;
using UnityEngine;
using System.Collections;

public class ChoiceDialogueState : IPNJState
{
    private PNJData _pnjData;
    private DialogueController _dialogueControllerSuccess;
    private DialogueController _dialogueControllerFailure;
    private PotionChoiceController _potionChoiceController;
    private PNJStateMachine _pnjStateMachine;

    public void EnterState(PNJData data)
    {
        _pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();
        _potionChoiceController = GameObject.FindObjectOfType<PotionChoiceController>();

        if (_pnjStateMachine != null)
        {
            _dialogueControllerSuccess = _pnjStateMachine.DialogueControllerSuccess;
            _dialogueControllerFailure = _pnjStateMachine.DialogueControllerFailure;
        }

        _pnjData = data;
        _pnjData.Agent.isStopped = true;
        _pnjData.Animator.SetBool("isWalking", false);

        if (_potionChoiceController.SelectedPotionName == _pnjStateMachine.SelectedPotionName)
        {
            _pnjStateMachine.ResetFailedAttempts();
            _dialogueControllerSuccess?.StartDialogue();
        }
        else
        {
            _pnjStateMachine.IncrementFailedAttempts();
            _dialogueControllerFailure?.StartDialogue();
        }

        DialogueController.OnDialogueFinished += OnDialogueEnd;
    }

    private void OnDialogueEnd()
    {
        DialogueController.OnDialogueFinished -= OnDialogueEnd;

        if (_potionChoiceController.SelectedPotionName == _pnjStateMachine.SelectedPotionName)
        {
            _pnjStateMachine.ResetFailedAttempts();
            _pnjStateMachine.NextState();
        }
        else
        {
            if (_pnjStateMachine.FailedAttempts >= 3)
            {
                _pnjStateMachine.NextState();
            }
            else
            {
                _dialogueControllerFailure.EndDialogue();
                DialogueController.OnDialogueFinished -= OnDialogueEnd;
                _pnjStateMachine.TransitionToState(3);
            }
        }
    }

    public void UpdateState(PNJData data, PNJStateMachine stateMachine) { }

    public void ExitState(PNJData data)
    {
        data.Agent.isStopped = false;
    }
}