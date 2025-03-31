using UnityEngine;
using NaughtyAttributes;

namespace MoonlitMixes.Potion
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion")]
    public class Potion : ScriptableObject
    {
        [SerializeField] private Recipe _recipe;
        [SerializeField, Range(1,4)] private int _quality;
        [SerializeField, ReadOnly] private int _price;
        [SerializeField] private Sprite _sprite;
        
        public Recipe Recipe
        {
            get => _recipe;
            set => _recipe = value;
        }

        public Sprite SpritePotion
        {
            get => _sprite;
        }
    }
}
