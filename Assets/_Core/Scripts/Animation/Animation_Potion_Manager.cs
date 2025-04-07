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
        private bool _isMix;

        protected override void GetRequiredComponent()
        {
            _playerHoldItem = GetComponent<PlayerHoldItem>();
        }

        protected override void UpdateOtherAnimations()
        {
            bool isHoldingItem = _playerHoldItem.ItemHold != null;
            _isPerformingActionHolding = isHoldingItem && !_isCut && !_isMix && !_isCrush;

            _animator.SetBool("isHoldingIdle", _isPerformingActionHolding && !isMoving);
            _animator.SetBool("isHoldingRun", _isPerformingActionHolding && isMoving);

            if(_isPerformingActionHolding || _isInventoryOpen || _isPerformingActionIdle)
            {
                _otherRestrictingAnim = true;
                _animator.SetBool("isRun", false);
                _animator.SetBool("isIdle", false);
            }
            else
            {
                _otherRestrictingAnim = false;
            }
        }

        public void OpenInventory()
        {
            _isInventoryOpen = true;
            _animator.SetBool("isLongIdle", true);
        }

        public void CloseInventory()
        {
            _isInventoryOpen = false;
            _animator.SetBool("isLongIdle", false);
        }

        public void InteractCut()
        {
            _isCut = true;
            _animator.SetBool("isCut", true);
        }

        public void FinishedInteractCut()
        {
            _isCut = false;
            _animator.SetBool("isCut", false);
        }

        public void InteractMix()
        {
            _isMix = true;
            _animator.SetBool("isMix", true);
            _isPerformingActionHolding = false;
            _animator.SetBool("isHoldingRun", false);
        }

        public void FinishedInteractMix()
        {
            _isMix = false;
            _animator.SetBool("isMix", false);
        }

        public void InteractCrush()
        {
            _isCrush = true;
            _animator.SetBool("isCrush", true);
        }

        public void FinishedInteractCrush()
        {
            _isCrush = false;
            _animator.SetBool("isCrush", false);
        }

        public void QuitInteractWithoutItem()
        {
            _isPerformingActionIdle = false;
            _isPerformingActionHolding = false;
        }

        public void QuitInteractWithItem()
        {
            _isPerformingActionIdle = false;
            _isPerformingActionHolding = true;
        }

        public void SetPerformingActionHolding(bool state)
        {
            _isPerformingActionHolding = state;
            _isPerformingActionIdle = !state;

            _animator.SetBool("isIdle", false);
            _animator.SetBool("isHoldingIdle", false);
            _animator.SetBool("isRun", false);
            _animator.SetBool("isHoldingRun", false);

            if (state)
            {
                _animator.SetBool("isHoldingIdle", true);
                _animator.SetBool("isHoldingRun", true);
            }
        }

        public void SetPerformingActionIdle(bool state)
        {
            Debug.Log("SetPerformingActionIdle");
            _isPerformingActionIdle = state;
            _isPerformingActionHolding = !state;

            _animator.SetBool("isHoldingIdle", false);
            _animator.SetBool("isHoldingRun", false);
            _animator.SetBool("isRun", false);

            if (state)
            {
                Debug.Log("SetPerformingActionIdleState");
                _animator.SetBool("isIdle", true);
                _isPerformingActionIdle = false;
            }
        }
    }
}