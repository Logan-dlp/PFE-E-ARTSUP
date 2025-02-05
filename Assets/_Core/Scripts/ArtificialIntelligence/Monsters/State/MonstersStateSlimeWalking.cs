using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonstersStateSlimeWalking : IMonstersState
    {
        public void Enter(MonstersData data)
        {
            data.NavMeshAgent.SetDestination(GenerateRandomPoint(data.InitialPosition, .1f, 5));
        }

        public IMonstersState Update(MonstersData data)
        {
            if (data.NavMeshAgent.hasPath)
            {
                Vector3 dir = (data.NavMeshAgent.steeringTarget - data.SlimeGameObject.transform.position).normalized;
                Vector3 animDir = data.SlimeGameObject.transform.InverseTransformDirection(dir);
                bool isFacingMoveDirection = Vector3.Dot(dir, data.SlimeGameObject.transform.forward) > .5f;
                
                data.Animator.SetFloat("Horizontal", isFacingMoveDirection ? animDir.x : 0, .5f, Time.deltaTime);
                data.Animator.SetFloat("Vertical", isFacingMoveDirection ? animDir.z : 0, .5f, Time.deltaTime);
                
                data.SlimeGameObject.transform.rotation = Quaternion.RotateTowards(data.SlimeGameObject.transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);

                if (Vector3.Distance(data.SlimeGameObject.transform.position, data.NavMeshAgent.destination) < data.NavMeshAgent.radius)
                {
                    data.NavMeshAgent.ResetPath();
                }
            }
            else
            {
                return new MonstersStateSlimeIdle();
            }
            
            if (data.PlayerGameObject != null)
            {
                return new MonstersStateSlimeFollowPlayer();
            }
            
            return null;
        }

        public void Exit(MonstersData data)
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