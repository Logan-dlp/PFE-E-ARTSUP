using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class StateSlimeIdle : IState
    {
        private float _timer;
        
        public void Enter(AStateMachineData data)
        {
            _timer = Random.Range(.5f, 3f);
        }

        public IState Update(AStateMachineData data)
        {
            SlimeData slimeData = data as SlimeData;
            
            slimeData.Animator.SetFloat("Horizontal", 0, .25f, Time.deltaTime);
            slimeData.Animator.SetFloat("Vertical", 0, .25f, Time.deltaTime);
            
            if (slimeData.InAttack)
            {
                return new StateSlimeAttack();
            }
            
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                return new StateSlimeWalking();
            }
            
            return null;
        }

        public void Exit(AStateMachineData data)
        {
            
        }
    }
}