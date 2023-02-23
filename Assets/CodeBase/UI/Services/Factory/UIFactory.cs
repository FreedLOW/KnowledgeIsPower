using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider assets;
        private readonly IStaticDataService staticData;
        private readonly IPersistentProgressService progressService;
        private readonly IAdsService adsService;

        private Transform uiRoot;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData, 
            IPersistentProgressService progressService, IAdsService adsService)
        {
            this.assets = assets;
            this.staticData = staticData;
            this.progressService = progressService;
            this.adsService = adsService;
        }

        public void CreateShop()
        {
            WindowConfig config = staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(config.Prefab, uiRoot) as ShopWindow;
            window.Construct(progressService, adsService);
        }

        public async Task CreateRoot()
        {
            GameObject root = await assets.Instantiate(AssetsAddress.UIRoot);
            uiRoot = root.transform;
        }
    }
}