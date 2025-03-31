using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "DayNightCycleInfo", menuName = "Scriptable Objects/DayNightCycleInfo")]
    public class DayNightCycleInfo : ScriptableObject
    {
        [SerializeField] private Material _skyboxDawn;
        [SerializeField] private Material _skyboxDay;
        [SerializeField] private Material _skyBoxAfternoon;
        [SerializeField] private Material _skyBoxTwillight;
        [SerializeField] private Material _skyboxNight;
        
        [SerializeField] private int _actualTimePhase;
        
        public Material skyboxDawn
        {
            get => _skyboxDawn;
            set => _skyboxDawn = value;
        }
        public Material skyboxDay
        {
            get => _skyboxDay;
            set => _skyboxDay = value;
        }
        public Material skyBoxAfternoon
        {
            get => _skyBoxAfternoon;
            set => _skyBoxAfternoon = value;
        }
        public Material skyBoxTwillight
        {
            get => _skyBoxTwillight;
            set => _skyBoxTwillight = value;
        }
        public Material skyboxNight
        {
            get => _skyboxNight;
            set => _skyboxNight = value;
        }
        public int ActualTimePhase
        {
            get => _actualTimePhase;
            set => _actualTimePhase = value;
        }
    }
}
