using UnityEngine;

namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class MoveToEndState : IPNJState
    {
        private bool _hasArrived = false;

        public void EnterState(PNJData data)
        {
            data.animator.SetBool("isWalking", true);
            data.agent.SetDestination(data.waypoints[data.waypoints.Count - 1].position);
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (!_hasArrived && !data.agent.pathPending && data.agent.remainingDistance <= data.agent.stoppingDistance)
            {
                _hasArrived = true;
                data.animator.SetBool("isWalking", false);
                data.agent.updateRotation = false;

                RotateLeft(data);

                return new DialogueState();
            }

            return null;
        }

        public void ExitState(PNJData data)
        {
            data.agent.updateRotation = true;
        }

        private void RotateLeft(PNJData data)
        {
            data.pnjGameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}