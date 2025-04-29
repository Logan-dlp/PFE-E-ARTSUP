namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public class DespawnState : IPNJState
    {
        public void EnterState(PNJData data)
        {
            data.StateMachine.InvokeOnDespawn();
            data.PNJGameObject.SetActive(false);
        }

        public IPNJState UpdateState(PNJData data)
        {
            return null;
        }

        public void ExitState(PNJData data) { }
    }
}