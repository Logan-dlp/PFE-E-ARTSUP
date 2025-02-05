using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class MonstersStateSlimeFollowPlayer : IMonstersState
    {
        public void Enter(MonstersData data)
        {
            if (data.PlayerGameObject != null)
            {
                data.NavMeshAgent.SetDestination(data.PlayerGameObject.transform.position);
            }
        }

        public IMonstersState Update(MonstersData data)
        {
            if (data.PlayerGameObject == null)
            {
                return new MonstersStateSlimeIdle();
            }
            
            if (Vector3.Distance(data.PlayerGameObject.transform.position, data.InitialPosition) > data.AttackRadius * 1.5f)
            {
                data.PlayerGameObject = null;
            }
            
            if (data.NavMeshAgent.hasPath)
            {
                Vector3 dir = (data.NavMeshAgent.steeringTarget - data.MonsterGameObject.transform.position).normalized;
                Vector3 animDir = data.MonsterGameObject.transform.InverseTransformDirection(dir);
                bool isFacingMoveDirection = Vector3.Dot(dir, data.MonsterGameObject.transform.forward) > .5f;
                
                data.Animator.SetFloat("Horizontal", isFacingMoveDirection ? animDir.x : 0, .5f, Time.deltaTime);
                data.Animator.SetFloat("Vertical", isFacingMoveDirection ? animDir.z : 0, .5f, Time.deltaTime);
                
                data.MonsterGameObject.transform.rotation = Quaternion.RotateTowards(data.MonsterGameObject.transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);

                if (Vector3.Distance(data.MonsterGameObject.transform.position, data.NavMeshAgent.destination) < data.NavMeshAgent.radius)
                {
                    data.NavMeshAgent.ResetPath();
                    return new MonstersStateSlimeAttack();
                }
            }
            
            if (data.PlayerGameObject != null)
            {
                data.NavMeshAgent.SetDestination(data.PlayerGameObject.transform.position);
            }
            
            return null;
        }

        public void Exit(MonstersData data)
        {
            data.NavMeshAgent.ResetPath();
        }
    }
}