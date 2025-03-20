using MoonlitMixes.Dialogue;

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
            
            if (DialogueController.Instance != null)
            {
                //_dialogueController.StartDialogue();
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