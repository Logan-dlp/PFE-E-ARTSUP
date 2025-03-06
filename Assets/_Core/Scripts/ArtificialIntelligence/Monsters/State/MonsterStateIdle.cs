using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonsterStateIdle : IMonsterState
    {
        private const float MIN_RANGE_RANDOM_TIMER = 0.5f;
        private const float MAX_RANGE_RANDOM_TIMER = 3;
        private const float DAMP_TIME = 0.25f;
        
        private const string HORIZONTAL_ANIMATOR_VARIABLE = "Horizontal";
        private const string VERTICAL_ANIMATOR_VARIABLE = "Vertical";
        
        private float _timer;
        
        public void Enter(MonsterData monsterData)
        {
            _timer = Random.Range(MIN_RANGE_RANDOM_TIMER, MAX_RANGE_RANDOM_TIMER);
        }

        public IMonsterState Update(MonsterData monsterData)
        {
            monsterData.Animator.SetFloat(HORIZONTAL_ANIMATOR_VARIABLE, 0, DAMP_TIME, Time.deltaTime);
            monsterData.Animator.SetFloat(VERTICAL_ANIMATOR_VARIABLE, 0, DAMP_TIME, Time.deltaTime);
            
            if (monsterData.PlayerReference != null)
            {
                return new MonsterStateFollowPlayer();
            }
            
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                return new MonsterStateWalking();
            }
            
            return null;
        }

        public void Exit(MonsterData monsterData)
        {
            
        }
    }
}