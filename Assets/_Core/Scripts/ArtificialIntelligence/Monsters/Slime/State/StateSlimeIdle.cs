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
                slimeData.NavMeshAgent.SetDestination(GenerateRandomPoint(slimeData.InitialPosition, .1f, 5));
                return new StateSlimeWalking();
            }
            
            return null;
        }

        public void Exit(AStateMachineData data)
        {
            
        }
        
        private Vector3 GenerateRandomPoint(Vector3 origin, float rayMin, float rayMax)
        {
            Vector2 randomCircle = Random.insideUnitCircle * Random.Range(rayMin, rayMax);
            Vector3 randomPoint = origin + new Vector3(randomCircle.x, 0, randomCircle.y);
            return randomPoint;
        }
    }
}