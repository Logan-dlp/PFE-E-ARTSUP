using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class ChoiceDialogueState : IPNJState
    {
        private PNJData _pnjData;
        private DialogueController _dialogueControllerSuccess;
        private DialogueController _dialogueControllerFailure;
        private PotionChoiceController _potionChoiceController;

        public void EnterState(PNJData data)
        {
            PNJStateMachine pnjStateMachine = data.PNJGameObject.GetComponent<PNJStateMachine>();
            _potionChoiceController = GameObject.FindObjectOfType<PotionChoiceController>();

            if (pnjStateMachine != null)
            {
                _dialogueControllerSuccess = pnjStateMachine.DialogueControllerSuccess;
                _dialogueControllerFailure = pnjStateMachine.DialogueControllerFailure;
            }

            _pnjData = data;
            _pnjData.Agent.isStopped = true;
            _pnjData.Animator.SetBool("isWalking", false);

            if (_potionChoiceController.SelectedPotionName == pnjStateMachine.SelectedPotionName)
            {
                if (_dialogueControllerSuccess != null)
                {
                    _dialogueControllerSuccess.StartDialogue();
                }
            }
            else
            {
                if (_dialogueControllerFailure != null)
                {
                    _dialogueControllerFailure.StartDialogue();
                }
            }

            DialogueController.OnDialogueFinished += OnDialogueEnd;
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

        public void ExitState(PNJData data)
        {
            data.Agent.isStopped = false;
        }
    }
}