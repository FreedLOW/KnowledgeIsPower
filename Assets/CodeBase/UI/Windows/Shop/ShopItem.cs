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
        public Image ItemImage;

        private IIAPService _iapService;
        private IAssetProvider _assets;
        private ProductDescription _productDescription;

        public void Construct(IIAPService iapService, IAssetProvider assets, ProductDescription productDescription)
        {
            _iapService = iapService;
            _assets = assets;
            _productDescription = productDescription;
        }

        public async void Initialize()
        {
            BuyItemButton.onClick.AddListener(OnBuyItem);

            PriceText.text = _productDescription.Config.Price;
            QuantityText.text = _productDescription.Config.Quantity.ToString();
            AvailableItemsLeftText.text = _productDescription.AvailablePurchasesLeft.ToString();
            ItemImage.sprite = await _assets.Load<Sprite>(_productDescription.Config.Icon);
        }

        private void OnBuyItem() => 
            _iapService.StartPurchase(_productDescription.Id);
    }
}