using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class SpawnState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.PNJGameObject.transform.position = data.Waypoints[0].position;
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine)
        {
            stateMachine.NextState();
        }

        public void ExitState(PNJData data) { }
    }
}