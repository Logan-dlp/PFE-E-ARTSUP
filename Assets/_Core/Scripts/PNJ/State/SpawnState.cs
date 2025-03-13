namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class SpawnState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.PNJGameObject.SetActive(true);
            data.PNJGameObject.transform.position = data.Waypoints[0].position;
            data.Agent.enabled = true;
            data.Animator.enabled = true;
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine)
        {
            stateMachine.NextState();
        }

        public void ExitState(PNJData data) { }
    }
}