namespace MoonlitMixes.AI.StateMachine.States
{
    public interface IState
    {
        public void Enter(AStateMachineData data);
        public IState Update(AStateMachineData data);
        public void Exit(AStateMachineData data);
    }
}