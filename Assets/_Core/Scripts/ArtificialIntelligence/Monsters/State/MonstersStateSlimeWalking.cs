using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonstersStateSlimeWalking : IMonstersState
    {
        private const float DAMP_TIME = 0.5f;
        private const float MAX_DEGREES_DELTA = 180;
        
        private const string HORIZONTAL_ANIMATOR_VARIABLE = "Horizontal";
        private const string VERTICAL_ANIMATOR_VARIABLE = "Vertical";
        
        public void Enter(MonstersData data)
        {
            data.NavMeshAgent.SetDestination(GenerateRandomPoint(data.InitialPosition, 0, data.AttackRadius));
        }

        public IMonstersState Update(MonstersData data)
        {
            if (data.NavMeshAgent.hasPath)
            {
                Vector3 dir = (data.NavMeshAgent.steeringTarget - data.MonsterGameObject.transform.position).normalized;
                Vector3 animDir = data.MonsterGameObject.transform.InverseTransformDirection(dir);
                bool isFacingMoveDirection = Vector3.Dot(dir, data.MonsterGameObject.transform.forward) > DAMP_TIME;
                
                data.Animator.SetFloat(HORIZONTAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDir.x : 0, DAMP_TIME, Time.deltaTime);
                data.Animator.SetFloat(VERTICAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDir.z : 0, DAMP_TIME, Time.deltaTime);
                
                data.MonsterGameObject.transform.rotation = Quaternion.RotateTowards(data.MonsterGameObject.transform.rotation, Quaternion.LookRotation(dir), MAX_DEGREES_DELTA * Time.deltaTime);

                if (Vector3.Distance(data.MonsterGameObject.transform.position, data.NavMeshAgent.destination) < data.NavMeshAgent.radius)
                {
                    data.NavMeshAgent.ResetPath();
                }
            }
            else
            {
                return new MonstersStateSlimeIdle();
            }
            
            if (data.PlayerReference != null)
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