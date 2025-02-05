namespace MoonlitMixes.AI.StateMachine.States
{
    public interface IMonstersState
    {
        public void Enter(MonstersData data);
        public IMonstersState Update(MonstersData data);
        public void Exit(MonstersData data);
    }
}