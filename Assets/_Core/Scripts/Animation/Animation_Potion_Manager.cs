using MoonlitMixes.Player;
using UnityEngine;

namespace MoonlitMixes.Animation
{
    public class AnimationPotionManager : AAnimationManager
    {
        private PlayerHoldItem _playerHoldItem;
        private bool _isPerformingActionHolding;
        private bool _isPerformingActionIdle;
        private bool _isInventoryOpen;
        private bool _isCut;
        private bool _isCrush;
        private bool _isStir;

        protected override void GetRequiredComponent()
        {
            _playerHoldItem = GetComponent<PlayerHoldItem>();
        }

        protected override void UpdateOtherAnimations()
        {
            bool isHoldingItem = _playerHoldItem.ItemHold != null;
            _isPerformingActionHolding = isHoldingItem && !_isCut && !_isStir && !_isCrush;

            if(_isPerformingActionHolding)
            {
                if(isMoving)
                {
                    _animator.SetBool("HoldingIdle", false);
                    _animator.SetBool("HoldingWalk", true);
                }
                else
                {
                    _animator.SetBool("HoldingWalk", false);
                    _animator.SetBool("HoldingIdle", true);
                }
            }

            if(_isPerformingActionHolding || _isInventoryOpen || _isPerformingActionIdle || _isCut || _isCrush || _isStir)
            {
                _otherRestrictingAnim = true;
            }
            else
            {
                _otherRestrictingAnim = false;
                _animator.SetBool("HoldingWalk", false);
                _animator.SetBool("HoldingIdle", false);
            }
        }

        public void OpenInventory()
        {
            _isInventoryOpen = true;
            EndAllIdleNRunAnim();
            _animator.SetBool("LongIdle", true);

        }

        public void CloseInventory()
        {
            _isInventoryOpen = false;
            _animator.SetBool("LongIdle", false);
        }

        public void InteractCut()
        {
            _isCut = true;
            EndAllIdleNRunAnim();
            _animator.SetBool("Cut", true);
            _isPerformingActionHolding = true;
        }

        public void FinishedInteractCut()
        {
            _isCut = false;
            _animator.SetBool("Cut", false);
        }

        public void InteractCauldronWithoutStir()
        {
            EndAllIdleNRunAnim();
            _animator.SetTrigger("Put");
        }

        public void InteractStir()
        {
            _isStir = true;
            EndAllIdleNRunAnim();
            _animator.SetBool("Mix", true);
        }

        public void FinishedInteractStir()
        {
            _isStir = false;
            _animator.SetBool("Mix", false);
        }

        public void InteractCrush()
        {
            _isCrush = true;
            EndAllIdleNRunAnim();
            _animator.SetBool("Crush", true);
        }

        public void FinishedInteractCrush()
        {
            _isCrush = false;
            _animator.SetBool("Crush", false);
        }

        public void TrashItem()
        {
            _animator.SetTrigger("Throw");
            QuitInteractWithoutItem();
        }

        public void PutItem()
        {
            EndAllIdleNRunAnim();
            _animator.SetTrigger("Put");
        }

        public void QuitInteractWithoutItem()
        {
            EndAllIdleNRunAnim();
            _isPerformingActionIdle = false;
            _isPerformingActionHolding = false;
        }

        public void QuitInteractWithItem()
        {
            EndAllIdleNRunAnim();
            _isPerformingActionIdle = false;
            _isPerformingActionHolding = true;
        }

        private void EndAllIdleNRunAnim()
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("HoldingIdle", false);
            _animator.SetBool("Run", false);
            _animator.SetBool("HoldingWalk", false);
        }
    }
}