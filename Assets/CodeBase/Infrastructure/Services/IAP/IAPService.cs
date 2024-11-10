using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.Progress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPService : IIAPService
    {
        private readonly IAPProvider _iapProvider;
        private readonly IPersistentProgressService _progressService;

        public bool IsInitialized => _iapProvider.IsInitialized;
        
        public event Action OnInitialized;

        public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
        {
            _iapProvider = iapProvider;
            _progressService = progressService;
        }

        public void Initialize()
        {
            _iapProvider.Initialize(this);
            _iapProvider.Initialized += () => OnInitialized?.Invoke();
        }

        public List<ProductDescription> Products() =>
            ProductDescriptions().ToList();

        public void StartPurchase(string productId) => 
            _iapProvider.StartPurchase(productId);

        public PurchaseProcessingResult ProcessPurchase(Product purchaseProduct)
        {
            ProductConfig productConfig = _iapProvider.Configs[purchaseProduct.definition.id];

            switch (productConfig.ItemType)
            {
                case ItemType.Skulls:
                    _progressService.PlayerProgress.WorldData.LootData.Add(productConfig.Quantity);
                    _progressService.PlayerProgress.PurchaseData.AddPurchase(purchaseProduct.definition.id);
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }

        private IEnumerable<ProductDescription> ProductDescriptions()
        {
            PurchaseData purchaseData = _progressService.PlayerProgress.PurchaseData;

            foreach (string productID in _iapProvider.Products.Keys)
            {
                ProductConfig config = _iapProvider.Configs[productID];
                Product product = _iapProvider.Products[productID];
                BoughtIAP boughtIAP = purchaseData.BoughtIAPs.Find(x => x.IAPid == productID);
                
                if (ProductBoughtOut(boughtIAP, config))
                    continue;

                yield return new ProductDescription
                {
                    Id = productID,
                    Product = product,
                    Config = config,
                    AvailablePurchasesLeft = boughtIAP != null ? config.MaxPurchaseCount - boughtIAP.Count : config.MaxPurchaseCount,
                };
            } 
        }

        private static bool ProductBoughtOut(BoughtIAP boughtIAP, ProductConfig config) => 
            boughtIAP != null && boughtIAP.Count >= config.MaxPurchaseCount;
    }
}