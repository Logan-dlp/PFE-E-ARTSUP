namespace MoonlitMixes.AI.StateMachine.States
{
    public interface IMonsterState
    {
        public void Enter(MonsterData data);
        public IMonsterState Update(MonsterData data);
        public void Exit(MonsterData data);
    }
}