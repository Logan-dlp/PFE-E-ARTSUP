using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class DespawnState : IPNJState
    {
        public void EnterState(PNJData pnj)
        {
            pnj.PNJGameObject.SetActive(false);
        }

        public void UpdateState(PNJData pnj, PNJStateMachine stateMachine)
        {
            stateMachine.NextState();
        }

        public void ExitState(PNJData pnj) { }
    }
}