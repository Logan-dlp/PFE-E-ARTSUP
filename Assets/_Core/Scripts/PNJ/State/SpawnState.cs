using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class SpawnState : IPNJState
    {
        public void EnterState(PNJData pnj)
        {
            pnj.PNJGameObject.transform.position = pnj.Waypoints[0].position;
        }

        public void UpdateState(PNJData pnj, PNJStateMachine stateMachine)
        {
            stateMachine.NextState();
        }

        public void ExitState(PNJData pnj) { }
    }
}