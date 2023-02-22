using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI coinText;
        public RewardedAdItem AdItem;

        public void Construct(IPersistentProgressService progressService, IAdsService adsService)
        {
            base.Construct(progressService);
            AdItem.Construct(adsService, progressService);
        }

        protected override void Initialize()
        {
            OnUpdateText();
            AdItem.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            PlayerProgress.WorldData.LootData.Changed += OnUpdateText;
            AdItem.Subscribe();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            PlayerProgress.WorldData.LootData.Changed -= OnUpdateText;
            AdItem.Cleanup();
        }

        private void OnUpdateText() => 
            coinText.text = PlayerProgress.WorldData.LootData.Collected.ToString();
    }
}