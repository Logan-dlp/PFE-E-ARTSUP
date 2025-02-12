using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonsterStateAttack : IMonsterState
    {
        private const string ATTACK_ANIMATOR_VARIABLE = "Attack";
        
        public void Enter(MonsterData data)
        {
            data.Animator.SetTrigger(ATTACK_ANIMATOR_VARIABLE);
        }

        public IMonsterState Update(MonsterData data)
        {
            if (data.PlayerReference == null)
            {
                return new MonsterStateIdle();
            }
            
            if (data.Attacking)
            {
                data.Animator.SetTrigger(ATTACK_ANIMATOR_VARIABLE);
                return new MonsterStateFollowPlayer();
            }
            
            return null;
        }

        public void Exit(MonsterData data)
        {
            data.Attacking = false;
        }
    }
}
