using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.IAP
{
    public interface IIAPService : IService
    {
        bool IsInitialized { get; }
        event Action OnInitialized;
        void Initialize();
        void StartPurchase(string productId);
        List<ProductDescription> Products();
    }
}