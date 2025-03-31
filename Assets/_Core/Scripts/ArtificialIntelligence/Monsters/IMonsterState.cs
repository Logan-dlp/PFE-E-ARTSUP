namespace MoonlitMixes.AI.StateMachine.States
{
    public interface IMonsterState
    {
        public void Enter(MonsterData monsterData);
        public IMonsterState Update(MonsterData monsterData);
        public void Exit(MonsterData monsterData);
    }
}