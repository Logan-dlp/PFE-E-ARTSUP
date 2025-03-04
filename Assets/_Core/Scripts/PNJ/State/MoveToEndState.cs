using UnityEngine;
using MoonlitMixes.AI.StateMachine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MoveToEndState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.Agent.isStopped = false;
            data.Animator.SetBool("isWalking", true);
            data.Agent.SetDestination(data.Waypoints[data.Waypoints.Count - 1].position);
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine)
        {
            if (!data.Agent.pathPending && data.Agent.remainingDistance <= data.Agent.stoppingDistance)
            {
                data.Animator.SetBool("isWalking", false);

                data.Agent.updateRotation = false;

                RotateLeft(data);

                stateMachine.NextState();
            }
        }

        public void ExitState(PNJData data)
        {
            data.Agent.updateRotation = true;
        }

        private void RotateLeft(PNJData data)
        {
            data.PNJGameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}