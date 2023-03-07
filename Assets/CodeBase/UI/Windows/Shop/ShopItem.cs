using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItem : MonoBehaviour
    {
        public Button BuyItemButton;
        public TextMeshProUGUI PriceText;
        public TextMeshProUGUI QuantityText;
        public TextMeshProUGUI AvailableItemsLeftText;
        public Image Icon;
        
        private IIAPService iapService;
        private IAssetProvider assets;

        private ProductDescription productDescription;

        public void Construct(IIAPService iapService, IAssetProvider assets, ProductDescription productDescription)
        {
            this.iapService = iapService;
            this.assets = assets;
            this.productDescription = productDescription;
        }

        public async void Initialize()
        {
            BuyItemButton.onClick.AddListener(OnBuyItem);
            
            PriceText.text = productDescription.ProductConfig.Price;
            QuantityText.text = productDescription.ProductConfig.Quantity.ToString();
            AvailableItemsLeftText.text = productDescription.AvailablePurchaseLeft.ToString();

            Icon.sprite = await assets.Load<Sprite>(productDescription.ProductConfig.Icon);
        }

        private void OnBuyItem() => 
            iapService.StartPurchase(productDescription.Id);
    }
}