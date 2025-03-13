namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public interface IPNJState
    {
        void EnterState(PNJData data);
        void UpdateState(PNJData data, PNJStateMachine stateMachine);
        void ExitState(PNJData data);
    }
}