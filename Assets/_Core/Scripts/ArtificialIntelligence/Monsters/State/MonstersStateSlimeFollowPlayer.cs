using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonstersStateSlimeFollowPlayer : IMonstersState
    {
        private const float ATTACK_REMOTENESS = 4.5f;
        private const float DAMP_TIME = 0.25f;
        private const float MAX_DEGREES_DELTA = 180;
        
        private const string HORIZONTAL_ANIMATOR_VARIABLE = "Horizontal";
        private const string VERTICAL_ANIMATOR_VARIABLE = "Vertical";
        
        public void Enter(MonstersData data)
        {
            if (data.PlayerReference != null)
            {
                data.NavMeshAgent.SetDestination(data.PlayerReference.transform.position);
            }
        }

        public IMonstersState Update(MonstersData data)
        {
            if (data.PlayerReference == null)
            {
                return new MonstersStateSlimeIdle();
            }
            
            if (Vector3.Distance(data.PlayerReference.transform.position, data.InitialPosition) > data.DetectionStop)
            {
                data.PlayerReference = null;
            }
            
            if (data.NavMeshAgent.hasPath)
            {
                Vector3 dir = (data.NavMeshAgent.steeringTarget - data.MonsterGameObject.transform.position).normalized;
                Vector3 animDir = data.MonsterGameObject.transform.InverseTransformDirection(dir);
                bool isFacingMoveDirection = Vector3.Dot(dir, data.MonsterGameObject.transform.forward) > DAMP_TIME;
                
                data.Animator.SetFloat(HORIZONTAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDir.x : 0, DAMP_TIME, Time.deltaTime);
                data.Animator.SetFloat(VERTICAL_ANIMATOR_VARIABLE, isFacingMoveDirection ? animDir.z : 0, DAMP_TIME, Time.deltaTime);
                
                data.MonsterGameObject.transform.rotation = Quaternion.RotateTowards(data.MonsterGameObject.transform.rotation, Quaternion.LookRotation(dir), MAX_DEGREES_DELTA * Time.deltaTime);

                if (Vector3.Distance(data.MonsterGameObject.transform.position, data.NavMeshAgent.destination) < data.NavMeshAgent.radius * ATTACK_REMOTENESS)
                {
                    data.NavMeshAgent.ResetPath();
                    return new MonstersStateSlimeAttack();
                }
            }
            
            if (data.PlayerReference != null)
            {
                data.NavMeshAgent.SetDestination(data.PlayerReference.transform.position);
            }
            
            return null;
        }

        public void Exit(MonstersData data)
        {
            data.NavMeshAgent.ResetPath();
        }
    }
}