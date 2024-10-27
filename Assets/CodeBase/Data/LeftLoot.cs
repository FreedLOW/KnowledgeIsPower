using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LeftLoot
    {
        public string Id;
        public Vector3Data Position;
        public Loot Loot;

        public LeftLoot(string id, Vector3Data position, Loot loot)
        {
            Id = id;
            Position = position;
            Loot = loot;
        }
    }
}