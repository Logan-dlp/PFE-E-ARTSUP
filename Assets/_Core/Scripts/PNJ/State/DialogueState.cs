using UnityEngine;
using MoonlitMixes.AI.StateMachine;
using System;
using MoonlitMixes.Potion;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class DialogueState : IPNJState
    {
        private DialogueController _dialogueController;
        private PNJData _pnjData;
        private PotionResult _selectedPotion;

        public void EnterState(PNJData data)
        {
            _pnjData = data;
            data.Agent.isStopped = true;
            data.Animator.SetBool("isWalking", false);

            if (_pnjData.RequestPotionList != null && _pnjData.RequestPotionList.PotionResults.Count > 0)
            {
                _selectedPotion = _pnjData.RequestPotionList.PotionResults[0];
                Debug.Log($"Potion sélectionnée : {_selectedPotion.Recipe.RecipeName}");
            }

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