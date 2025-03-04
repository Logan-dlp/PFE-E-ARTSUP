using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class DespawnState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.PNJGameObject.SetActive(false);
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine)
        {
            stateMachine.NextState();
        }

        public void ExitState(PNJData data) { }
    }
}