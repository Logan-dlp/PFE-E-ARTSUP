using UnityEngine;
using System.Collections.Generic;
using MoonlitMixes.Potion;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "PotionListData", menuName = "Scriptable Objects/PotionListData")]
    public class PotionListData : ScriptableObject
    {
        [SerializeField] private List<PotionResult> potionResults = new List<PotionResult>();

        public List<PotionResult> PotionResults
        {
            get => potionResults;
            set => potionResults = value;
        }
    }
}