using MoonlitMixes.Dialogue;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class DialogueState : IPNJState
    {
        private PNJData _pnjData;

        public void EnterState(PNJData data)
        {
            _pnjData = data;
            data.Agent.isStopped = true;
            data.Animator.SetBool("isWalking", false);

            PNJStateMachine pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();

            if (DialogueController.Instance != null && pnjStateMachine.BeginDialogueData != null)
            {
                DialogueController.Instance.StartDialogue(pnjStateMachine.BeginDialogueData);
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
}