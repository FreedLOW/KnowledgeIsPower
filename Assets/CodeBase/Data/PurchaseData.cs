using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseData
    {
        public List<BoughtIAPs> BoughtIAPs = new List<BoughtIAPs>();

        public Action OnBoughtIAP;

        public void AddPurchase(string id)
        {
            BoughtIAPs boughtIAPs = Product(id);

            if (boughtIAPs != null)
            {
                boughtIAPs.Count++;
            }
            else
            {
                BoughtIAPs.Add(new BoughtIAPs() {IAPId = id, Count = 1});
            }
            
            OnBoughtIAP?.Invoke();
        }

        private BoughtIAPs Product(string id)
        {
            return BoughtIAPs.Find(x => x.IAPId == id);
        }
    }
}