using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData.Window;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        private Transform _uiRoot;
        private IAdsService _adsService;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, 
            IAdsService adsService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
        }

        public void CreateUIRoot() => 
            _uiRoot = _assets.Instantiate(UIRootPath).transform;

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            var window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
            window.Construct(_adsService, _progressService);
        }
    }
}