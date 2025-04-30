namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class MoveToStartState : IPNJState
    {
        private bool _hasArrived = false;

        public void EnterState(PNJData data)
        {
            data.Agent.SetDestination(data.Waypoints[0].position);
            data.Animator.SetBool("isWalking", true);
        }

        public IPNJState UpdateState(PNJData data)
        {
            if (!_hasArrived && !data.Agent.pathPending && data.Agent.remainingDistance <= data.Agent.stoppingDistance)
            {
                _hasArrived = true;
                data.Animator.SetBool("isWalking", false);

                return new DespawnState();
            }

            return null;
        }

        public void ExitState(PNJData data) { }
    }
}