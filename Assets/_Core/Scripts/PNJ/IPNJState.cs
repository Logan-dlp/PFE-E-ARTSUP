namespace MoonlitMixes.AI.PNJ.StateMachine.States
{
    public interface IPNJState
    {
        void EnterState(PNJData data);
        IPNJState UpdateState(PNJData data);
        void ExitState(PNJData data);
    }
}