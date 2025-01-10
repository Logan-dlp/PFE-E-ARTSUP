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
            Debug.Log($"Enter in {this.GetType().Name}");
            _timer = 0;
        }

        public IState Update(AStateMachineData data)
        {
            Debug.Log($"Update in {this.GetType().Name}");
            SlimeData slimeData = data as SlimeData;
            
            if (slimeData.TargetPosition == null)
            {
                slimeData.NavMeshAgent.SetDestination(GenerateRandomPoint(slimeData.InitialPosition, .1f, 5));
            }
            
            if (Vector3.Distance(slimeData.SlimeGameObject.transform.position, slimeData.TargetPosition) > .05f)
            {
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                }
                else
                {
                    slimeData.NavMeshAgent.SetDestination(GenerateRandomPoint(slimeData.InitialPosition, .1f, 5));
                    _timer = Random.Range(.1f, 5);
                }
                
            }
            
            if (slimeData.InAttack)
            {
                return new StateSlimeAttack();
            }
            
            return null;
        }

        public void Exit(AStateMachineData data)
        {
            Debug.Log($"Exit in {this.GetType().Name}");
        }

        private Vector3 GenerateRandomPoint(Vector3 origin, float rayMin, float rayMax)
        {
            Vector2 randomCircle = Random.insideUnitCircle * Random.Range(rayMin, rayMax);
            Vector3 randomPoint = origin + new Vector3(randomCircle.x, 0, randomCircle.y);
            return randomPoint;
        }
    }
}