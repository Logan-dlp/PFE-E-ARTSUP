using UnityEngine;

namespace MoonlitMixes.AI
{
    public class TargetTest : MonoBehaviour
    {
        [SerializeField] private Monsters _monsters;

        [ContextMenu("Attack Monster")]
        public void AttackMonsters()
        {
            _monsters.Attacked(this);
        }
    }
}