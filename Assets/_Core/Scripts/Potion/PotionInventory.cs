using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Potion
{
    public class PotionInventory : MonoBehaviour
    {
        public List<Potion> _potionList = new List<Potion>();
        [SerializeField] private Image[] _images;
        
        public List<Potion> PotionList 
        {
            get => _potionList;
        }

        public void UpdatePotionCanvas()
        {
            for(int i = 0; i < _potionList.Count; i++)
            {
                _images[i].color = new Color(1, 1, 1, 1);
                _images[i].sprite = _potionList[i].Recipe.PotionSprite;
            }
        }
    }
}
