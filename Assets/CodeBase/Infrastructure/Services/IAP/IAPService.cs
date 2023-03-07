using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPService : IIAPService
    {
        private readonly InAppProvider inAppProvider;
        private readonly IPersistentProgressService progressService;

        public bool IsInitialized => inAppProvider.IsInitialized;

        public event Action OnInitialized;

        public IAPService(InAppProvider inAppProvider, IPersistentProgressService progressService)
        {
            this.inAppProvider = inAppProvider;
            this.progressService = progressService;
        }

        public void Initialize()
        {
            inAppProvider.Initialize(this);
            inAppProvider.OnInitialize += () => OnInitialized?.Invoke();
        }

        public void StartPurchase(string productId) => 
            inAppProvider.StartPurchase(productId);

        public PurchaseProcessingResult ProcessPurchase(Product purchasedProduct)
        {
            ProductConfig productConfig = inAppProvider.ProductConfigs[purchasedProduct.definition.id];

            switch (productConfig.ItemType)
            {
                case ItemType.Skulls:
                    progressService.PlayerProgress.WorldData.LootData.Add(productConfig.Quantity);
                    progressService.PlayerProgress.PurchaseData.AddPurchase(purchasedProduct.definition.id);
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }

        public List<ProductDescription> Products() =>
            ProductsDescription().ToList();

        private IEnumerable<ProductDescription> ProductsDescription()
        {
            PurchaseData purchaseData = progressService.PlayerProgress.PurchaseData;

            foreach (var productId in inAppProvider.Products.Keys)
            {
                ProductConfig productConfig = inAppProvider.ProductConfigs[productId];
                Product product = inAppProvider.Products[productId];

                BoughtIAPs boughtIAPs = purchaseData.BoughtIAPs.Find(x => x.IAPId == productId);

                if (ProductBoughtOut(boughtIAPs, productConfig))
                    continue;

                yield return new ProductDescription()
                {
                    Id = productId,
                    ProductConfig = productConfig,
                    Product = product,
                    AvailablePurchaseLeft = boughtIAPs != null ? 
                        productConfig.MaxPurchaseCount - boughtIAPs.Count : 
                        productConfig.MaxPurchaseCount,
                };
            } 
        }

        private static bool ProductBoughtOut(BoughtIAPs boughtIAPs, ProductConfig productConfig) => 
            boughtIAPs != null && boughtIAPs.Count >= productConfig.MaxPurchaseCount;
    }
}