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
            if(_rouletteSelectionTools.ToolSlots.Length == 0 || _rouletteSelectionTools.ToolGameObjects.Count == 0) _animator.SetBool("HasTool", false);
            else _animator.SetBool("HasTool", true);

            if(_playerMovement.CurrentSpeed > _playerMovement.WalkSpeed) 
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
    }
}