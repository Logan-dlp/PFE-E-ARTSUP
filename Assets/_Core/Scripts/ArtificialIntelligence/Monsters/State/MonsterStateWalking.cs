using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonsterStateWalking : IMonsterState
    {
        private const float DAMP_TIME = 0.5f;
        private const float MAX_DEGREES_DELTA = 180;
        
        private const string HORIZONTAL_ANIMATOR_VARIABLE = "Horizontal";
        private const string VERTICAL_ANIMATOR_VARIABLE = "Vertical";
        
        public void Enter(MonsterData monsterData)
        {
            if (monsterData.NavMeshAgent != null)
            {
                monsterData.NavMeshAgent.SetDestination(GenerateRandomPoint(monsterData.InitialPosition, 0, monsterData.AttackRadius));
            }
        }

        public IMonsterState Update(MonsterData monsterData)
        {
            if (monsterData.NavMeshAgent.hasPath)
            {
                Vector3 dir = (monsterData.NavMeshAgent.steeringTarget - monsterData.MonsterGameObject.transform.position).normalized;
                Vector3 animDir = monsterData.MonsterGameObject.transform.InverseTransformDirection(dir);
                bool isFacingMoveDirection = Vector3.Dot(dir, monsterData.MonsterGameObject.transform.forward) > DAMP_TIME;
                
                monsterData.Animator.SetFloat(HORIZONTAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDir.x : 0, DAMP_TIME, Time.deltaTime);
                monsterData.Animator.SetFloat(VERTICAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDir.z : 0, DAMP_TIME, Time.deltaTime);
                
                monsterData.MonsterGameObject.transform.rotation = Quaternion.RotateTowards(monsterData.MonsterGameObject.transform.rotation, Quaternion.LookRotation(dir), MAX_DEGREES_DELTA * Time.deltaTime);

                if (Vector3.Distance(monsterData.MonsterGameObject.transform.position, monsterData.NavMeshAgent.destination) < monsterData.NavMeshAgent.radius)
                {
                    monsterData.NavMeshAgent.ResetPath();
                }
            }
            else
            {
                return new MonsterStateIdle();
            }
            
            if (monsterData.PlayerReference != null)
            {
                return new MonsterStateFollowPlayer();
            }
            
            return null;
        }

        public void Exit(MonsterData monsterData)
        {
            monsterData.NavMeshAgent.velocity = Vector3.zero;
        }
        
        private Vector3 GenerateRandomPoint(Vector3 origin, float rayMin, float rayMax)
        {
            Vector2 randomCircle = Random.insideUnitCircle * Random.Range(rayMin, rayMax);
            Vector3 randomPoint = origin + new Vector3(randomCircle.x, 0, randomCircle.y);
            return randomPoint;
        }
    }
}