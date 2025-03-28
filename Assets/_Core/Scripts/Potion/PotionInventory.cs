using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Potion
{
    public class PotionInventory : MonoBehaviour
    {
        public List<Potion> _potionList = new List<Potion>();

        [SerializeField] private GameObject _potionSlotPrefab;
        [SerializeField] private GameObject _UI;
        
        public List<Potion> PotionList 
        {
            get => _potionList;
        }

        [ContextMenu("UpdatePotions")]
        public void UpdatePotionCanvas()
        {
            foreach(Transform child in _UI.transform)
            {
                Destroy(child.gameObject);
            }

            foreach(Potion potion in _potionList)
            {
                GameObject obj = Instantiate(_potionSlotPrefab, _UI.transform);
                obj.GetComponent<Image>().sprite = potion.SpritePotion;
            }
        }
    }
}
