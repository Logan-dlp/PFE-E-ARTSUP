namespace MoonlitMixes.AI.StateMachine
{
    public interface IPNJState
    {
        void EnterState(PNJData pnj);
        void UpdateState(PNJData pnj, PNJStateMachine stateMachine);
        void ExitState(PNJData pnj);
    }
}