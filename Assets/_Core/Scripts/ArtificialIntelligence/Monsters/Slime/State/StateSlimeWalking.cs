using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class StateSlimeWalking : IState
    {
        public void Enter(AStateMachineData data)
        {
            
        }

        public IState Update(AStateMachineData data)
        {
            SlimeData slimeData = data as SlimeData;
            
            if (slimeData.NavMeshAgent.hasPath)
            {
                Vector3 dir = (slimeData.NavMeshAgent.steeringTarget - slimeData.SlimeGameObject.transform.position).normalized;
                Vector3 animDir = slimeData.SlimeGameObject.transform.InverseTransformDirection(dir);
                
                slimeData.Animator.SetFloat("Horizontal", animDir.x, .5f, Time.deltaTime);
                slimeData.Animator.SetFloat("Vertical", animDir.z, .5f, Time.deltaTime);
                
                slimeData.SlimeGameObject.transform.rotation = Quaternion.RotateTowards(slimeData.SlimeGameObject.transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
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
    }
}