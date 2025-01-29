using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class StateSlimeWalking : IState
    {
        public void Enter(AStateMachineData data)
        {
            SlimeData slimeData = data as SlimeData;
            
            slimeData.NavMeshAgent.SetDestination(GenerateRandomPoint(slimeData.InitialPosition, .1f, 5));
        }

        public IState Update(AStateMachineData data)
        {
            SlimeData slimeData = data as SlimeData;
            
            if (slimeData.NavMeshAgent.hasPath)
            {
                Vector3 dir = (slimeData.NavMeshAgent.steeringTarget - slimeData.SlimeGameObject.transform.position).normalized;
                Vector3 animDir = slimeData.SlimeGameObject.transform.InverseTransformDirection(dir);
                bool isFacingMoveDirection = Vector3.Dot(dir, slimeData.SlimeGameObject.transform.forward) > .5f;
                
                slimeData.Animator.SetFloat("Horizontal", isFacingMoveDirection ? animDir.x : 0, .5f, Time.deltaTime);
                slimeData.Animator.SetFloat("Vertical", isFacingMoveDirection ? animDir.z : 0, .5f, Time.deltaTime);
                
                slimeData.SlimeGameObject.transform.rotation = Quaternion.RotateTowards(slimeData.SlimeGameObject.transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);

                if (Vector3.Distance(slimeData.SlimeGameObject.transform.position, slimeData.NavMeshAgent.destination) < slimeData.NavMeshAgent.radius)
                {
                    slimeData.NavMeshAgent.ResetPath();
                }
            }
            else
            {
                return new StateSlimeIdle();
            }
            
            if (slimeData.InAttack)
            {
                return new StateSlimeAttack();
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