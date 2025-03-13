namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class DespawnState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.PNJGameObject.SetActive(false);
        }

        public void UpdateState(PNJData data, PNJStateMachine stateMachine) { }

        public void ExitState(PNJData data) { }
    }
}