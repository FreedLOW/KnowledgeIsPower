using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseData
    {
        public List<BoughtIAP> BoughtIAPs = new List<BoughtIAP>();

        public Action OnChanged;
        
        public void AddPurchase(string id)
        {
            BoughtIAP boughtIAP = GetProduct(id);

            if (boughtIAP != null)
            {
                boughtIAP.Count++;
            }
            else
            {
                BoughtIAPs.Add(new BoughtIAP() { IAPid = id, Count = 1 });
            }
            
            OnChanged?.Invoke();
        }

        private BoughtIAP GetProduct(string id) => 
            BoughtIAPs.Find(x => x.IAPid == id);
    }
}