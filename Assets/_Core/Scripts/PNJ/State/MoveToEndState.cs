using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MoveToEndState : IPNJState
    {
        public void EnterState(PNJData pnj)
        {
            pnj.Agent.isStopped = false;
            pnj.Animator.SetBool("isWalking", true);
            pnj.Agent.SetDestination(pnj.Waypoints[pnj.Waypoints.Count - 1].position);
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