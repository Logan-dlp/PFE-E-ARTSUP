using UnityEngine;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    
    public class Slime : AMonsters
    {
        [SerializeField] private bool _active;
        
        private void Start()
        {
            base._data = new SlimeData()
            {
                
            };
            
            TransitionTo(new StateSlimeIdle());
        }
    }
}