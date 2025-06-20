using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "DayNightCycleInfo", menuName = "Scriptable Objects/DayNightCycleInfo")]
    public class DayNightCycleInfo : ScriptableObject
    {       
        [SerializeField] private int _actualTimePhase;
        [SerializeField] private int _actualDay;

        public int ActualDay
        {
            get => _actualDay;
            set => _actualDay = Mathf.Clamp(value, 0, 2);
        }
        
        public int ActualTimePhase
        {
            get => _actualTimePhase;
            set => _actualTimePhase = Mathf.Clamp(value, 0, 3);
        }
    }
}