using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.AI.PNJ;
using MoonlitMixes.Dialogue;
using UnityEngine;

public class DialogueState : IPNJState
{
    private PNJData _pnjData;

    public void EnterState(PNJData data)
    {
        _pnjData = data;
        data.Agent.isStopped = true;
        data.Animator.SetBool("isWalking", false);

        PNJStateMachine pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();

        if (DialogueController.Instance != null && pnjStateMachine.dialogueData != null)
        {
            DialogueController.Instance.StartDialogue(pnjStateMachine.dialogueData);
            DialogueController.OnDialogueFinished += OnDialogueEnd;
        }
        else
        {
            Debug.LogWarning("DialogueController ou DialogueData est nul");
        }
    }

    private void OnDialogueEnd()
    {
        DialogueController.OnDialogueFinished -= OnDialogueEnd;

        if (_pnjData != null)
        {
            _pnjData.PNJGameObject.GetComponent<PNJStateMachine>().NextState();
        }
    }

    public void UpdateState(PNJData data, PNJStateMachine stateMachine) { }
    public void ExitState(PNJData data) { }
}