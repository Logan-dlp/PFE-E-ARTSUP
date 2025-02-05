using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonstersStateSlimeAttack : IMonstersState
    {
        public void Enter(MonstersData data)
        {
            data.Animator.SetTrigger("Attack");
        }

        public IMonstersState Update(MonstersData data)
        {
            if (data.PlayerGameObject == null)
            {
                return new MonstersStateSlimeIdle();
            }
            
            if (data.Attacking)
            {
                data.Animator.SetTrigger("Attack");
                return new MonstersStateSlimeFollowPlayer();
            }
            
            return null;
        }

        public void Exit(MonstersData data)
        {
            data.Attacking = false;
        }
    }
}
