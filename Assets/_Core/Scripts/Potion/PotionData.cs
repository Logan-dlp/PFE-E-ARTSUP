using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private string _description;

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
}