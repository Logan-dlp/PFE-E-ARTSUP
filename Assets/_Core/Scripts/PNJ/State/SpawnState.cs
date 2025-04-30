namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class SpawnState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.Agent.enabled = true;
            data.Animator.enabled = true;
        }

        public IPNJState UpdateState(PNJData data)
        {
            return new MoveToEndState();
        }

        public void ExitState(PNJData data){}
    }
}