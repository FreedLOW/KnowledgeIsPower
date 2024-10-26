using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId,MonsterStaticData> _monsters = new();

        public void LoadMonsters() =>
            _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(t => t.MonsterTypeId, t => t);

        public MonsterStaticData GetMonster(MonsterTypeId typeId) => 
            _monsters.GetValueOrDefault(typeId, null);
    }
}