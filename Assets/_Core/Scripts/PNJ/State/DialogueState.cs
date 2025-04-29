using MoonlitMixes.Dialogue;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class DialogueState : IPNJState
    {
        private bool _dialogueFinished = false;

        public void EnterState(PNJData data)
        {
            data.Agent.isStopped = true;
            data.Animator.SetBool("isWalking", false);

            if (DialogueController.Instance != null && data.StateMachine.BeginDialogueData != null)
            {
                DialogueController.Instance.StartDialogue(data.StateMachine.BeginDialogueData);
                DialogueController.OnDialogueFinished += OnDialogueEnd;
            }
            else
            {
                _dialogueFinished = true;
            }
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (_dialogueFinished)
            {
                DialogueController.OnDialogueFinished -= OnDialogueEnd;
                return new ChoosePotionState();
            }

            return null;
        }

        public void ExitState(PNJData data) { }

        private void OnDialogueEnd()
        {
            _dialogueFinished = true;
        }
    }
}