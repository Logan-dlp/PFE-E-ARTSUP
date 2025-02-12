using UnityEngine;

namespace MoonlitMixes.AI
{
    public class TargetTest : MonoBehaviour
    {
        [SerializeField] private Monster monster;

        [ContextMenu("Attack Monster")]
        public void AttackMonsters()
        {
            monster.Attacked(this);
        }
    }
}