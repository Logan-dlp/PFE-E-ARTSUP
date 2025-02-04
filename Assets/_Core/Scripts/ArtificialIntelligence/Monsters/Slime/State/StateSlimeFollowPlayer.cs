using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class StateSlimeFollowPlayer : IState
    {
        public void Enter(AStateMachineData data)
        {
            Debug.Log("Enter Follow Player");
            
            SlimeData slimeData = data as SlimeData;
            
            if (slimeData.PlayerGameObject != null)
            {
                slimeData.NavMeshAgent.SetDestination(slimeData.PlayerGameObject.transform.position);
            }
        }

        public IState Update(AStateMachineData data)
        {
            SlimeData slimeData = data as SlimeData;

            if (slimeData.PlayerGameObject == null)
            {
                return new StateSlimeIdle();
            }
            
            if (Vector3.Distance(slimeData.PlayerGameObject.transform.position, slimeData.InitialPosition) > slimeData.AttackRadius * 1.5f)
            {
                slimeData.PlayerGameObject = null;
            }
            
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
                    Debug.Log("Attack");
                }
            }
            
            return null;
        }

        public void Exit(AStateMachineData data)
        {
            SlimeData slimeData = data as SlimeData;
            
            slimeData.NavMeshAgent.ResetPath();
            
            Debug.Log("Exit Follow Player");
        }
    }
}