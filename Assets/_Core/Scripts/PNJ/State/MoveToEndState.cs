using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class MoveToEndState : IPNJState
    {
        private bool _hasArrived = false;

        public void EnterState(PNJData data)
        {
            data.Animator.SetBool("isWalking", true);
            data.Agent.SetDestination(data.Waypoints[data.Waypoints.Count - 1].position);
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (!_hasArrived && !data.Agent.pathPending && data.Agent.remainingDistance <= data.Agent.stoppingDistance)
            {
                _hasArrived = true;
                data.Animator.SetBool("isWalking", false);
                data.Agent.updateRotation = false;

                RotateLeft(data);

                return new DialogueState();
            }

            return null;
        }

        public void ExitState(PNJData data)
        {
            data.Agent.updateRotation = true;
        }

        private void RotateLeft(PNJData data)
        {
            data.PnjGameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}