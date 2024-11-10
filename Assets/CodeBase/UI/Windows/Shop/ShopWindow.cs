using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.Progress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        public RewardedAdItem AdItem;
        public ShopItemsContainer ShopItemsContainer;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService,
            IIAPService iapService, IAssetProvider assets)
        {
            base.Construct(progressService);
            AdItem.Construct(adsService, progressService);
            ShopItemsContainer.Construct(iapService, progressService, assets);
        }

        protected override void Initialize()
        {
            RefreshSkullText();
            AdItem.Initialize();
            ShopItemsContainer.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            Progress.WorldData.LootData.OnCollected += RefreshSkullText;
            AdItem.Subscribe();
            ShopItemsContainer.Subscribe();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.WorldData.LootData.OnCollected -= RefreshSkullText;
            AdItem.Cleanup();
            ShopItemsContainer.Cleanup();
        }

        private void RefreshSkullText() => 
            SkullText.text = Progress.WorldData.LootData.Collected.ToString();
    }
}