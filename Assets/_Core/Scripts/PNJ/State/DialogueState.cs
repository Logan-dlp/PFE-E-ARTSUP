using UnityEngine;
using System;
using MoonlitMixes.Potion;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
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

            PNJStateMachine pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();

            if (pnjStateMachine != null)
            {
                _dialogueController = pnjStateMachine.DialogueController;
            }

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