namespace MoonlitMixes.Interactions
{
    using ExplorationTools;
    
    public class TreeInteractionObject : InteractionObject
    {
        private TreeHealth _treeHealth;
        private bool _uiEnabledLastFrame = false;

        protected override void Awake()
        {
            _treeHealth = GetComponent<TreeHealth>();
            base.Awake();
        }

        private void Update()
        {
            if (_uiEnabledLastFrame && _treeHealth != null && !_treeHealth.CanChop())
            {
                DisableUI();
                _uiEnabledLastFrame = false;
            }
        }

        public override void EnableUI()
        {
            if (_treeHealth != null || _treeHealth.CanChop())
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