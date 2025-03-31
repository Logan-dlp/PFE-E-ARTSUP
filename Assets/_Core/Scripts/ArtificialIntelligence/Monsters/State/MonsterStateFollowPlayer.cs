using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonsterStateFollowPlayer : IMonsterState
    {
        private const float DAMP_TIME = 0.25f;
        private const float MAX_DEGREES_DELTA = 180;
        
        private const string HORIZONTAL_ANIMATOR_VARIABLE = "Horizontal";
        private const string VERTICAL_ANIMATOR_VARIABLE = "Vertical";
        
        public void Enter(MonsterData monsterData)
        {
            if (Vector3.Distance(monsterData.PlayerReference.transform.position, monsterData.InitialPosition) > monsterData.DetectionStop)
            {
                monsterData.PlayerReference = null;
            }
        }

        public IMonsterState Update(MonsterData monsterData)
        {
            if (monsterData.PlayerReference == null)
            {
                return new MonsterStateIdle();
            }
            else
            {
                monsterData.NavMeshAgent.SetDestination(monsterData.PlayerReference.transform.position);
            }
            
            if (Vector3.Distance(monsterData.PlayerReference.transform.position, monsterData.InitialPosition) > monsterData.DetectionStop)
            {
                monsterData.PlayerReference = null;
            }
            else
            {
                if (Vector3.Distance(monsterData.MonsterGameObject.transform.position, monsterData.PlayerReference.transform.position) < monsterData.StopDistanceToAttack)
                {
                    monsterData.Animator.SetFloat(HORIZONTAL_ANIMATOR_VARIABLE, 0);
                    monsterData.Animator.SetFloat(VERTICAL_ANIMATOR_VARIABLE, 0);

                    monsterData.NavMeshAgent.ResetPath();
                    
                    return new MonsterStateAttack();
                }
                else
                {
                    if (monsterData.NavMeshAgent.hasPath)
                    {
                        Vector3 direction = (monsterData.NavMeshAgent.steeringTarget - monsterData.MonsterGameObject.transform.position).normalized;
                        Vector3 animDirection = monsterData.MonsterGameObject.transform.InverseTransformDirection(direction);
                        bool isFacingMoveDirection = Vector3.Dot(direction, monsterData.MonsterGameObject.transform.forward) > DAMP_TIME;
                
                        monsterData.Animator.SetFloat(HORIZONTAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDirection.x : 0, DAMP_TIME, Time.deltaTime);
                        monsterData.Animator.SetFloat(VERTICAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDirection.z : 0, DAMP_TIME, Time.deltaTime);
                
                        monsterData.MonsterGameObject.transform.rotation = Quaternion.RotateTowards(monsterData.MonsterGameObject.transform.rotation, Quaternion.LookRotation(direction), MAX_DEGREES_DELTA * Time.deltaTime);
                    }
                }
            }
            
            return null;
        }

        public void Exit(MonsterData monsterData)
        {
            monsterData.NavMeshAgent.velocity = Vector3.zero;
        }
    }
}