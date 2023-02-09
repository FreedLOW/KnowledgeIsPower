using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class LeftLoot
    {
        public List<string> IdLeftLoots = new List<string>();
        public List<Loot> Loots = new List<Loot>();
    }
}