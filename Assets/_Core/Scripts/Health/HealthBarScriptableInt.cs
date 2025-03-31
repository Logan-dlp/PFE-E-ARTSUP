using System;
using UnityEngine;

namespace MoonlitMixes.Health
{
    [CreateAssetMenu(fileName = "HealthBarScriptableInt", menuName = "Scriptable Objects/HealthBarScriptableInt")]
    public class HealthBarScriptableInt : ScriptableObject
    {
        public Action<float> HealthAmountAction;

        public void SendHealthAmount(float healthAmount)
        {
            HealthAmountAction?.Invoke(healthAmount);
        }
    }
}