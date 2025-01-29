using MoonlitMixes.Item;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public abstract class ACookingMachine : MonoBehaviour
    {
        [SerializeField] protected ItemUsage _transformType;
        public ItemUsage TransformType => _transformType;

        public abstract void TogleShowInteractivity();

        public virtual ItemData ConvertItem(ItemData item)
        {
            return item.ItemToConvert;
        }
    }
}