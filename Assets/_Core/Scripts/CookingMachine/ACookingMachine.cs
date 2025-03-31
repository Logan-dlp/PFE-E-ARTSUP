using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.Datas;
using MoonlitMixes.Datas.QTE;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using MoonlitMixes.Player;

namespace MoonlitMixes.CookingMachine
{
    public abstract class ACookingMachine : MonoBehaviour
    {
        [SerializeField] protected ItemUsage _transformType;
        public ItemUsage TransformType => _transformType;
        
        [SerializeField] protected ScriptableQTEConfig _scriptableQTEConfig;
        [SerializeField] protected QuickTimeEvent _qTE;
        [SerializeField] protected ScriptableQTEEvent _scriptableQTEEvent;
        [SerializeField] protected ScriptableBoolEvent _scriptableBoolEvent;
        [SerializeField] protected Image _imageQTE;
        [SerializeField] protected Image _imageProgressBar;

        protected PlayerInteraction _playerInteraction;
        
        private ItemData _itemData;
        
        public abstract void TogleShowInteractivity();

        protected void Activate()
        {
            _scriptableBoolEvent.BoolAction += CheckItem;
        }

        protected void Desactivate()
        {
            _scriptableBoolEvent.BoolAction -= CheckItem;
        }

        public virtual void CheckItem(bool boolValue)
        {
            if(boolValue)
            {
                SuccesItem();
            }
            else
            {
                FailedItem();
            }
        }

        public virtual void ConvertItem(ItemData item, PlayerInteraction player)
        {
            Activate();
            _itemData = item;
            _scriptableQTEConfig.ScriptableBoolEvent = _scriptableBoolEvent;
            _scriptableQTEConfig.QteSlot = _imageQTE;
            _scriptableQTEConfig.ProgressBarUI = _imageProgressBar;
            _scriptableQTEEvent.SendObject(_scriptableQTEConfig);
            _playerInteraction = player;
        }

        public virtual void SuccesItem()
        {
            Desactivate();
            _playerInteraction.PlayerHoldItem.ChangeItemData(_itemData.ItemToConvert.ItemPrefab);
        }

        public virtual void FailedItem()
        {
            Desactivate();
            _playerInteraction.PlayerHoldItem.RemoveItem();
        }

        public void DesactiveQTE()
        {
            _qTE.EndQTE();
        }
    }
}