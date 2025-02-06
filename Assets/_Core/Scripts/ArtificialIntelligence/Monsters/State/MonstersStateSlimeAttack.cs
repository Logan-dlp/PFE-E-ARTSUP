using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonstersStateSlimeAttack : IMonstersState
    {
        private const string ATTACK_ANIMATOR_VARIABLE = "Attack";
        
        public void Enter(MonstersData data)
        {
            data.Animator.SetTrigger(ATTACK_ANIMATOR_VARIABLE);
        }

        public IMonstersState Update(MonstersData data)
        {
            if (data.PlayerReference == null)
            {
                return new MonstersStateSlimeIdle();
            }
            
            if (data.Attacking)
            {
                data.Animator.SetTrigger(ATTACK_ANIMATOR_VARIABLE);
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
