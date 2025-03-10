using System.Collections.Generic;
using MoonlitMixes.Item;
using UnityEngine;

namespace MoonlitMixes.Potion
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private string _recipeName;
        [SerializeField] private List<ItemData> _requiredIngredients;
        [SerializeField] private Sprite _potionSprite;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private Potion _potion;

        public string RecipeName
        {
            get => _recipeName;
        }

        public List<ItemData> RequiredIngredients
        {
            get => _requiredIngredients;
        }

        public Sprite PotionSprite
        {
            get => _potionSprite;
        }

        public string Description
        {
            get => _description;
        }

        public Potion Potion
        {
            get => _potion;
        }
    }
}