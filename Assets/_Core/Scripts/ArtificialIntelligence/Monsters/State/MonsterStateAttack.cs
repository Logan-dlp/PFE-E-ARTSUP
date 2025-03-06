using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonsterStateAttack : IMonsterState
    {
        private const float MAX_DEGREES_DELTA = 180;
        private const string ATTACK_ANIMATOR_VARIABLE = "Attack";
        
        public void Enter(MonsterData monsterData)
        {
            monsterData.Animator.SetTrigger(ATTACK_ANIMATOR_VARIABLE);
            monsterData.FinishedAttacking = false;
        }

        public IMonsterState Update(MonsterData monsterData)
        {
            Vector3 dir = (monsterData.PlayerReference.transform.position - monsterData.MonsterGameObject.transform.position).normalized;
            monsterData.MonsterGameObject.transform.rotation = Quaternion.RotateTowards(monsterData.MonsterGameObject.transform.rotation, Quaternion.LookRotation(dir), MAX_DEGREES_DELTA * Time.deltaTime);
            
            if (monsterData.PlayerReference == null)
            {
                return new MonsterStateIdle();
            }
            
            if (monsterData.FinishedAttacking)
            {
                return new MonsterStateFollowPlayer();
            }
            
            return null;
        }

        public void Exit(MonsterData monsterData)
        {
            
        }
    }
}
