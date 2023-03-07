using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItemsContainer : MonoBehaviour
    {
        public GameObject[] ShopUnavailableObjects;
        public Transform Parent;
        
        private readonly List<GameObject> shopItems = new List<GameObject>();

        private IIAPService iapService;
        private IPersistentProgressService progress;
        private IAssetProvider assetProvider;

        public void Construct(IIAPService iapService, IPersistentProgressService progress,
            IAssetProvider assetProvider)
        {
            this.iapService = iapService;
            this.progress = progress;
            this.assetProvider = assetProvider;
        }

        public void Initialize() => 
            RefreshAvailableItems();

        public void Subscribe()
        {
            iapService.OnInitialized += RefreshAvailableItems;
            progress.PlayerProgress.PurchaseData.OnBoughtIAP += RefreshAvailableItems;
        }

        public void Cleanup()
        {
            iapService.OnInitialized -= RefreshAvailableItems;
            progress.PlayerProgress.PurchaseData.OnBoughtIAP -= RefreshAvailableItems;
        }

        private async void RefreshAvailableItems()
        {
            UpdateShopUnavailableObjects();

            if (!iapService.IsInitialized)
                return;

            ClearShopItems();
            await FillShopItems();
        }

        private void ClearShopItems()
        {
            foreach (GameObject item in shopItems)
            {
                Destroy(item);
            }
        }

        private async Task FillShopItems()
        {
            foreach (ProductDescription productDescription in iapService.Products())
            {
                GameObject shopItemObject = await assetProvider.Instantiate(AssetsAddress.ShopItem, Parent);

                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();
                shopItem.Construct(iapService, assetProvider, productDescription);
                shopItem.Initialize();

                shopItems.Add(shopItemObject);
            }
        }

        private void UpdateShopUnavailableObjects()
        {
            foreach (GameObject unavailableObject in ShopUnavailableObjects)
            {
                unavailableObject.SetActive(!iapService.IsInitialized);
            }
        }
    }
}