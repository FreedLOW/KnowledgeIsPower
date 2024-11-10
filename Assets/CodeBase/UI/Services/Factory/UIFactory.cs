using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData.Window;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootAddress = "UIRoot";
        
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;
        private readonly IIAPService _iapService;

        private Transform _uiRoot;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, 
            IAdsService adsService, IIAPService iapService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
            _iapService = iapService;
        }

        public async Task CreateUIRoot()
        {
            var root = await _assets.Instantiate(UIRootAddress);
            _uiRoot = root.transform;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            var window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
            window.Construct(_adsService, _progressService, _iapService, _assets);
        }
    }
}