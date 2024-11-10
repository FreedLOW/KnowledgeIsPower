using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.Progress;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItemsContainer  : MonoBehaviour
    {
        private const string ShopItemAddress = "ShopItem";
        public GameObject[] ShopUnavailableObjects;
        public Transform Parent;

        private readonly List<GameObject> _shopItems = new();
        
        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssetProvider _assets;

        public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssetProvider assets)
        {
            _iapService = iapService;
            _progressService = progressService;
            _assets = assets;
        }

        public void Initialize() => 
            RefreshAvailableItems();

        public void Subscribe()
        {
            _iapService.OnInitialized += RefreshAvailableItems;
            _progressService.PlayerProgress.PurchaseData.OnChanged += RefreshAvailableItems;
        }

        public void Cleanup()
        {
            _iapService.OnInitialized -= RefreshAvailableItems;
            _progressService.PlayerProgress.PurchaseData.OnChanged -= RefreshAvailableItems;
        }

        private async void RefreshAvailableItems()
        {
            UpdateUnavailableObjects();

            if (!_iapService.IsInitialized)
                return;

            ClearShopItems();

            await FillShopItems();
        }

        private void UpdateUnavailableObjects()
        {
            foreach (GameObject shopUnavailableObject in ShopUnavailableObjects)
            {
                shopUnavailableObject.SetActive(!_iapService.IsInitialized);
            }
        }

        private void ClearShopItems()
        {
            foreach (GameObject shopItem in _shopItems)
            {
                Destroy(shopItem);
            }

            _shopItems.Clear();
        }

        private async Task FillShopItems()
        {
            foreach (ProductDescription productDescription in _iapService.Products())
            {
                GameObject shopItemObject = await _assets.Instantiate(ShopItemAddress, Parent);
                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();
                
                shopItem.Construct(_iapService, _assets, productDescription);
                shopItem.Initialize();
                
                _shopItems.Add(shopItemObject);
            }
        }
    }
}