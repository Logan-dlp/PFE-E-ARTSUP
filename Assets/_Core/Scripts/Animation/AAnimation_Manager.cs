using MoonlitMixes.Player;
using UnityEngine;

namespace MoonlitMixes.Animation
{
    public abstract class AAnimationManager : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        
        protected PlayerMovement _playerMovement;
        protected bool _otherRestrictingAnim;
        protected bool isMoving;
        
        protected abstract void UpdateOtherAnimations();

        protected abstract void GetRequiredComponent();
        
        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            GetRequiredComponent();
        }

        private void FixedUpdate()
        {
            isMoving = _playerMovement.TargetMovement.magnitude > 0.1f;
            
            UpdateOtherAnimations();
            
            if(!_otherRestrictingAnim)
            {
                UpdateBaseAnimations();
            }

        }

        private void UpdateBaseAnimations()
        {
            if(isMoving)
            {
                _animator.SetTrigger("Run");
            }
            else
            {
                _animator.SetTrigger("Idle");
            }
        }
    }
}
