using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;
        public List<LeftLoot> LeftLoots = new();

        public Action OnCollected;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            OnCollected?.Invoke();
        }

        public void AddLeftLoot(string id, Vector3Data position, Loot loot)
        {
            if (LeftLoots.Exists(l => l.Id == id))
                return;
            
            LeftLoots.Add(new LeftLoot(id, position, loot));
        }

        public void RemoveLeftLoot(string id)
        {
            if (LeftLoots.Count == 0)
                return;
            
            if (LeftLoots.Exists(l => l.Id == id))
            {
                LeftLoot leftLoot = LeftLoots.First(l => l.Id == id);
                LeftLoots.Remove(leftLoot);
            }
        }
    }
}