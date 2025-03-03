using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MoveToStartState : IPNJState
    {
        public void EnterState(PNJData pnj)
        {
            pnj.Agent.SetDestination(pnj.Waypoints[0].position);
            pnj.Animator.SetBool("isWalking", true);
        }

        public void UpdateState(PNJData pnj, PNJStateMachine stateMachine)
        {
            if (!pnj.Agent.pathPending && pnj.Agent.remainingDistance <= pnj.Agent.stoppingDistance)
            {
                pnj.Animator.SetBool("isWalking", false);
                stateMachine.NextState();
            }
        }

        public void ExitState(PNJData pnj) { }
    }
}