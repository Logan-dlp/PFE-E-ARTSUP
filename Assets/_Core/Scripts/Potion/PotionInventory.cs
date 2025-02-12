using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.Potion
{
    public class PotionInventory : MonoBehaviour
    {
        public List<Potion> _potionList;
        [SerializeField] private GameObject _canvas;
        
        public List<Potion> PotionList 
        {
            get => _potionList;
        }

        private void Update()
        {
            if(PotionList.Count > 0)
            {
                _canvas.SetActive(true);
            }
        }
    }
}
