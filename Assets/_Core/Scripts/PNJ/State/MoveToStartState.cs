namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class MoveToStartState : IPNJState
    {
        private bool _hasArrived = false;

        public void EnterState(PNJData data)
        {
            data.agent.SetDestination(data.waypoints[0].position);
            data.animator.SetBool("isWalking", true);
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (!_hasArrived && !data.agent.pathPending && data.agent.remainingDistance <= data.agent.stoppingDistance)
            {
                _hasArrived = true;
                data.animator.SetBool("isWalking", false);

                return new DespawnState();
            }

            return null;
        }

        public void ExitState(PNJData data) { }
    }
}