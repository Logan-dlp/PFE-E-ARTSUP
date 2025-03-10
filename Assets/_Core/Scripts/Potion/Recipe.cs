using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.Potion
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
    public class Recipe : ScriptableObject
    {
        [System.Serializable]
        public struct IngredientRequirement
        {
            public ElementType ElementType; 
            public int Quantity; 
        }

        [SerializeField] private string _recipeName;
        [SerializeField] private List<IngredientRequirement> _requiredIngredients;
        [SerializeField] private Sprite _potionSprite;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private Potion _potion;

        public string RecipeName
        {
            get => _recipeName;
        }

        public List<IngredientRequirement> RequiredIngredients
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