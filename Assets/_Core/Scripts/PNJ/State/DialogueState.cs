using MoonlitMixes.Dialogue;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class DialogueState : IPNJState
    {
        private bool _dialogueFinished = false;

        public void EnterState(PNJData data)
        {
            data.agent.isStopped = true;
            data.animator.SetBool("isWalking", false);

            if (DialogueController.Instance != null && data.beginDialogueData != null)
            {
                DialogueController.Instance.StartDialogue(data.beginDialogueData);
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