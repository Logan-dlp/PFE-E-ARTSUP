namespace MoonlitMixes.AI.StateMachine
{
    public interface IPNJState
    {
        void EnterState(PNJData data);
        void UpdateState(PNJData data, PNJStateMachine stateMachine);
        void ExitState(PNJData data);
    }
}