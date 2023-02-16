using TMPro;

namespace CodeBase.UI.Windows
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI coinText;

        protected override void Initialize() => 
            OnUpdateText();

        protected override void SubscribeUpdates() => 
            PlayerProgress.WorldData.LootData.Changed += OnUpdateText;

        protected override void Cleanup()
        {
            base.Cleanup();
            PlayerProgress.WorldData.LootData.Changed -= OnUpdateText;
        }

        private void OnUpdateText() => 
            coinText.text = PlayerProgress.WorldData.LootData.Collected.ToString();
    }
}