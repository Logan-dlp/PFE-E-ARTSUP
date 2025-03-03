using UnityEngine;
using System.Collections;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class DialogueState : IPNJState
    {
        public void EnterState(PNJData pnj)
        {
            pnj.PNJGameObject.GetComponent<MonoBehaviour>().StartCoroutine(DialogueWait(pnj));
        }

        private IEnumerator DialogueWait(PNJData pnj)
        {
            yield return new WaitForSeconds(pnj.DialogueDuration);
            pnj.PNJGameObject.GetComponent<PNJStateMachine>().NextState();
        }

        public void UpdateState(PNJData pnj, PNJStateMachine stateMachine) { }

        public void ExitState(PNJData pnj) { }
    }
}