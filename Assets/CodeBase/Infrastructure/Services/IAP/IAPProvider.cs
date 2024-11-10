using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPProvider : IDetailedStoreListener
    {
        private const string IAPProductsPath = "IAP/products";

        private IAPService _iapService;
        
        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public Dictionary<string, ProductConfig> Configs { get; private set; }
        public Dictionary<string, Product> Products { get; private set; }

        public bool IsInitialized => _controller != null && _extensions != null;
        
        public event Action Initialized;

        public void Initialize(IAPService iapService)
        {
            _iapService = iapService;
            
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            Configs = new Dictionary<string, ProductConfig>();
            Products = new Dictionary<string, Product>();
            
            LoadConfigs();
            
            foreach (ProductConfig product in Configs.Values)
            {
                builder.AddProduct(product.Id, product.ProductType);
            }
            
            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

            foreach (Product product in _controller.products.all)
            {
                Products.Add(product.definition.id, product);
            }
            
            Initialized?.Invoke();
            Debug.Log("InAPP initialize completed!");
        }

        public void StartPurchase(string productId) => 
            _controller.InitiatePurchase(productId);

        public void OnInitializeFailed(InitializationFailureReason error) => 
            Debug.LogError($"InAPP initialize failed: {error}");

        public void OnInitializeFailed(InitializationFailureReason error, string message) => 
            Debug.LogError($"InAPP initialize failed: {message}, with error: {error}");

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"Purchase success {purchaseEvent.purchasedProduct.definition.id}");
            return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) => 
            Debug.LogError($"Purchase failed: {failureReason} on product: {product.definition.id}, transaction id: {product.transactionID}");

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription) => 
            Debug.LogError($"Purchase failed: {failureDescription} on product: {product.definition.id}, transaction id: {product.transactionID}");

        private void LoadConfigs() =>
            Configs = Resources.Load<TextAsset>(IAPProductsPath)
                .text
                .ToDeserialized<ProductConfigWrapper>()
                .Configs
                .ToDictionary(x => x.Id, x => x);
    }
}