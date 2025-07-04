using System;

namespace MoonlitMixes.ExplorationTools
{
    public interface IDamageable
    {
        bool CanInteract();
        event Action OnBecameUnusable;
    }
}