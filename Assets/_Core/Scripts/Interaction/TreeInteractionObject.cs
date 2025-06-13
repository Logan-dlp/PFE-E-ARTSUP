using MoonlitMixes.ExplorationTools;

namespace MoonlitMixes.Interactions
{
    public class TreeInteractionObject : InteractionObject
    {
        private bool _uiEnabledLastFrame = false;

        private void Update()
        {
            TreeHealth treeHealth = GetComponent<TreeHealth>();

            if (_uiEnabledLastFrame && treeHealth != null && !treeHealth.CanChop())
            {
                DisableUI();
                _uiEnabledLastFrame = false;
            }
        }

        public override void EnableUI()
        {
            TreeHealth treeHealth = GetComponent<TreeHealth>();

            if (treeHealth == null || treeHealth.CanChop())
            {
                base.EnableUI();
                _uiEnabledLastFrame = true;
            }
        }

        public override void DisableUI()
        {
            base.DisableUI();
            _uiEnabledLastFrame = false;
        }
    }
}