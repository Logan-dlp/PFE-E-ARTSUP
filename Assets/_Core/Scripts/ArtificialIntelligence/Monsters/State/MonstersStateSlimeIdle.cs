using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonstersStateSlimeIdle : IMonstersState
    {
        private float _timer;
        
        public void Enter(MonstersData data)
        {
            _timer = Random.Range(.5f, 3f);
        }

        public IMonstersState Update(MonstersData data)
        {
            data.Animator.SetFloat("Horizontal", 0, .25f, Time.deltaTime);
            data.Animator.SetFloat("Vertical", 0, .25f, Time.deltaTime);
            
            if (data.PlayerGameObject != null)
            {
                return new MonstersStateSlimeFollowPlayer();
            }
            
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                return new MonstersStateSlimeWalking();
            }
            
            return null;
        }

        public void Exit(MonstersData data)
        {
            
        }
    }
}