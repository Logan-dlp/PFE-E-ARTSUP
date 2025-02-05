using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using MoonlitMixes.QTE;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public abstract class ACookingMachine : MonoBehaviour
    {
        [SerializeField] protected ItemUsage _transformType;
        [SerializeField] protected ScriptableQTEConfig _scriptableQTEConfig;
        [SerializeField] protected QuickTimeEvent _qTE;
        [SerializeField] protected ScriptableQTEEvent _scriptableQTEEvent;
        [SerializeField] protected ScriptableBoolEvent _scriptableBoolEvent;
        private ItemData itemData;

        public ItemUsage TransformType => _transformType;
        public abstract void TogleShowInteractivity();

        private void Activate()
        {
            _scriptableBoolEvent.BoolAction += CheckItem;
        }

        private void Desactivate()
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

        public virtual void ConvertItem(ItemData item)
        {
            
            Activate();
            itemData = item;
            _scriptableQTEEvent.SendObject(_scriptableQTEConfig);
        }

        public virtual ItemData SuccesItem()
        {
            Desactivate();
            return itemData.ItemToConvert;
        }

        public virtual void FailedItem()
        {
            Desactivate();
        }
    }
}