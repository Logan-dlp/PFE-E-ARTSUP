using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonsterStateAttack : IMonsterState
    {
        private const float MAX_DEGREES_DELTA = 180;
        private const string ATTACK_ANIMATOR_VARIABLE = "Attack";
        
        public void Enter(MonsterData data)
        {
            data.Animator.SetTrigger(ATTACK_ANIMATOR_VARIABLE);
        }

        public IMonsterState Update(MonsterData data)
        {
            Vector3 dir = (data.PlayerReference.transform.position - data.MonsterGameObject.transform.position).normalized;
            data.MonsterGameObject.transform.rotation = Quaternion.RotateTowards(data.MonsterGameObject.transform.rotation, Quaternion.LookRotation(dir), MAX_DEGREES_DELTA * Time.deltaTime);
            
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
