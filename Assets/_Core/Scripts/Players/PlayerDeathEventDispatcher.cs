namespace MoonlitMixes.Events
{
    public static class PlayerDeathEventDispatcher
    {
        public static event System.Action OnPlayerDeath;

        public static void TriggerDeath()
        {
            OnPlayerDeath?.Invoke();
        }
    }
}