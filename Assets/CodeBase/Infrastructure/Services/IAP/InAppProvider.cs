using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class InAppProvider : IStoreListener
    {
        private const string ProductPath = "IAP/products";

        private IAPService iapService;
        
        private IStoreController storeController;
        private IExtensionProvider extensionProvider;

        public Dictionary<string, ProductConfig> ProductConfigs { get; private set; }
        public Dictionary<string, Product> Products { get; private set; }
        public bool IsInitialized => storeController != null && extensionProvider != null;
        public event Action OnInitialize;

        public void Initialize(IAPService iapService)
        {
            this.iapService = iapService;
            
            ProductConfigs = new Dictionary<string, ProductConfig>();
            Products = new Dictionary<string, Product>();
            
            Load();
            
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (ProductConfig config in ProductConfigs.Values)
            {
                builder.AddProduct(config.Id, config.ProductType);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void StartPurchase(string productId) => 
            storeController.InitiatePurchase(productId);

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            extensionProvider = extensions;

            foreach (Product product in controller.products.all)
            {
                Products.Add(product.definition.id, product);
            }

            OnInitialize?.Invoke();
            Debug.Log("InAppPurchasing is initialized");
        }

        public void OnInitializeFailed(InitializationFailureReason error) => 
            Debug.LogError($"InAppPurchasing initialize failed with error {error}");

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"Purchasing is success {purchaseEvent.purchasedProduct.definition.id}");

            return iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) => 
            Debug.LogError($"Product {product.definition.id} purchase failed with error {failureReason}, transaction id {product.transactionID}");

        private void Load() =>
            ProductConfigs = Resources.Load<TextAsset>(ProductPath)
                .text
                .ToDeserialized<ProductConfigWrapper>()
                .Configs
                .ToDictionary(x => x.Id, x => x);
    }
}
