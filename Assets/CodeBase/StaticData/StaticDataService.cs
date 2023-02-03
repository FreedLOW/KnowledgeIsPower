using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string MonsterStaticDataPath = "StaticData/Monsters";

        private Dictionary<MonsterTypeId, MonsterStaticData> monsters;

        public void LoadMonsters()
        {
            monsters = Resources
                .LoadAll<MonsterStaticData>(MonsterStaticDataPath)
                .ToDictionary(x=>x.MonsterTypeId, x=>x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
            monsters.TryGetValue(typeId, out MonsterStaticData staticData) 
                ? staticData 
                : null;
    }
}