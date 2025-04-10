namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class MoveToStartState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.Agent.SetDestination(data.Waypoints[0].position);
            data.Animator.SetBool("isWalking", true);
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine)
        {
            if (!data.Agent.pathPending && data.Agent.remainingDistance <= data.Agent.stoppingDistance)
            {
                data.Animator.SetBool("isWalking", false);
                stateMachine.NextState();
            }
        }

        public void ExitState(PNJData data) { }
    }
}