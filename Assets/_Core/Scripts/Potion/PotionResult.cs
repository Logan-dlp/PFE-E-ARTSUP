using UnityEngine;

namespace MoonlitMixes.Potion
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion")]
    public class PotionResult : ScriptableObject
    {
        [SerializeField] private Recipe _recipe;
        [SerializeField, Range(1,4)] private int _quality;
        [SerializeField] private int _price;
        
        public Recipe Recipe
        {
            get => _recipe;
            set => _recipe = value;
        }

        public int Price
        {
            get => _price;
            set => _price = value;
        }
    }
}