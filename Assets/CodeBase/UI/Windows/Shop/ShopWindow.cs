using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.Progress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        public RewardedAdItem AdItem;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            base.Construct(progressService);
            AdItem.Construct(adsService, progressService);
        }

        protected override void Initialize()
        {
            RefreshSkullText();
            AdItem.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            Progress.WorldData.LootData.OnCollected += RefreshSkullText;
            AdItem.Subscribe();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.WorldData.LootData.OnCollected -= RefreshSkullText;
            AdItem.Cleanup();
        }

        private void RefreshSkullText() => 
            SkullText.text = Progress.WorldData.LootData.Collected.ToString();
    }
}