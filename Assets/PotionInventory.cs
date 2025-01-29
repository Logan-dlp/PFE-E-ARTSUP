using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.Potion
{
    public class PotionInventory : MonoBehaviour
    {
        [SerializeField] public List<Potion> _potionList;
        public List<Potion> PotionList 
        {
            get => _potionList;
        }
    }
}
