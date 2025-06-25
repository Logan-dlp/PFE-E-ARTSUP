using UnityEngine;
namespace MoonlitMixes.Animation
{
    public class AnimationExplorationManager : AAnimationManager
    {
        RouletteSelectionTools _rouletteSelectionTools;
        protected override void GetRequiredComponent()
        {
            _rouletteSelectionTools = FindFirstObjectByType<RouletteSelectionTools>();
        }

        protected override void UpdateOtherAnimations()
        {
            if (_rouletteSelectionTools.ToolSlots.Length == 0 || _rouletteSelectionTools.ToolGameObjects.Count == 0) _animator.SetBool("HasTool", false);
            else _animator.SetBool("HasTool", true);

            if (_playerMovement.CurrentSpeed > _playerMovement.WalkSpeed)
            {
                _animator.SetBool("Sprint", true);
                _animator.SetBool("Run", false);
                _otherRestrictingAnim = true;
            }
            else
            {
                _animator.SetBool("Sprint", false);
                _otherRestrictingAnim = false;
            }
        }
        public void SetIdle(bool value)
        {
            _animator.SetBool("Idle",value);
        }
        public void SetWalk(bool value)
        {
            _animator.SetBool("Run", value);
        }
        public void SetRun(bool value)
        {
            _animator.SetBool("Sprint", value);
        }
        public void UsePickaxe()
        {
            _animator.SetTrigger("UsePickaxe");
        }

        public void UseMachete()
        {
            _animator.SetTrigger("UseMachete");
        }

        public void UseStaff()
        {
            _animator.SetTrigger("AttackStaff");
        }

        public void Interaction()
        {
            _animator.SetTrigger("Interact");
        }

        public void Hit()
        {
            _animator.SetTrigger("Hit");
        }

        public void Death()
        {
            _animator.SetTrigger("Death");
        }
        public void StandUp(bool b)
        {
            _animator.SetBool("StandUp",b);
        }
        public void ChangeSpeed(float value)
        {
            _animator.speed = value;
        }
    }
}