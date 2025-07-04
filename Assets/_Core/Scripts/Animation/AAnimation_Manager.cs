using MoonlitMixes.Player;
using UnityEngine;

namespace MoonlitMixes.Animation
{
    public abstract class AAnimationManager : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        public Animator Animator { get { return _animator; } }
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

            if (!_otherRestrictingAnim)
            {
                UpdateBaseAnimations();
            }
        }

        private void UpdateBaseAnimations()
        {
            _animator.SetBool("Idle", !isMoving);
            _animator.SetBool("Run", isMoving);
        }

        public void DefaultState()
        {
            _animator.SetTrigger("Default");
        }

        public void SetIdle(bool value)
        {
            _animator.SetBool("Idle", value);
        }

        public void SetWalk(bool value)
        {
            _animator.SetBool("Run", value);
        }
        
        public void SetRun(bool value)
        {
            _animator.SetBool("Sprint", value);
        }
    }
}
