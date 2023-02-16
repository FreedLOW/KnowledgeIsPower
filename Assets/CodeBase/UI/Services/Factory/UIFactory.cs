using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider assets;
        private readonly IStaticDataService staticData;
        private readonly IPersistentProgressService progressService;

        private Transform uiRoot;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            this.assets = assets;
            this.staticData = staticData;
            this.progressService = progressService;
        }

        public void CreateShop()
        {
            WindowConfig config = staticData.ForWindow(WindowId.Shop);
            WindowBase window = Object.Instantiate(config.Prefab, uiRoot);
            window.Construct(progressService);
        }

        public void CreateRoot() => 
            uiRoot = assets.Instantiate(AssetsPath.UIRootPath).transform;
    }
}