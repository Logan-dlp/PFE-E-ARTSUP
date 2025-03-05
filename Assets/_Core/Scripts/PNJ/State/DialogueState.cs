using UnityEngine;
using MoonlitMixes.AI.StateMachine;
using System;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class DialogueState : IPNJState
    {
        private DialogueController _dialogueController;
        private PNJData _pnjData;

        public void EnterState(PNJData data)
        {
            _pnjData = data;
            data.Agent.isStopped = true;
            data.Animator.SetBool("isWalking", false);

            _dialogueController = GameObject.FindObjectOfType<DialogueController>();

            if (_dialogueController != null)
            {
                _dialogueController.StartDialogue();
                DialogueController.OnDialogueFinished += OnDialogueEnd;
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
}