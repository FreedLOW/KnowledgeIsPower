using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI coinText;
        public RewardedAdItem AdItem;
        public ShopItemsContainer ShopItemsContainer;

        public void Construct(IPersistentProgressService progressService, IAdsService adsService,
            IIAPService iapService, IAssetProvider assetProvider)
        {
            base.Construct(progressService);
            AdItem.Construct(adsService, progressService);
            ShopItemsContainer.Construct(iapService, progressService, assetProvider);
        }

        protected override void Initialize()
        {
            OnUpdateText();
            AdItem.Initialize();
            ShopItemsContainer.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            PlayerProgress.WorldData.LootData.Changed += OnUpdateText;
            AdItem.Subscribe();
            ShopItemsContainer.Subscribe();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            PlayerProgress.WorldData.LootData.Changed -= OnUpdateText;
            AdItem.Cleanup();
            ShopItemsContainer.Cleanup();
        }

        private void OnUpdateText() => 
            coinText.text = PlayerProgress.WorldData.LootData.Collected.ToString();
    }
}