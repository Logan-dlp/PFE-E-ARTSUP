using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonsterStateIdle : IMonsterState
    {
        private float _timer;
        
        public void Enter(MonsterData data)
        {
            _timer = Random.Range(.5f, 3f);
        }

        public IMonsterState Update(MonsterData data)
        {
            data.Animator.SetFloat("Horizontal", 0, .25f, Time.deltaTime);
            data.Animator.SetFloat("Vertical", 0, .25f, Time.deltaTime);
            
            if (data.PlayerReference != null)
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

        public void Exit(MonsterData data)
        {
            
        }
    }
}