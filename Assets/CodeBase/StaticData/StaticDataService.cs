using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string MonsterStaticDataPath = "StaticData/Monsters";
        private const string LevelStaticDataPath = "StaticData/Levels";

        private Dictionary<MonsterTypeId, MonsterStaticData> monsters;
        private Dictionary<string, LevelStaticData> levels;

        public void LoadMonsters()
        {
            monsters = Resources
                .LoadAll<MonsterStaticData>(MonsterStaticDataPath)
                .ToDictionary(x=>x.MonsterTypeId, x=>x);
            
            levels = Resources
                .LoadAll<LevelStaticData>(LevelStaticDataPath)
                .ToDictionary(x=>x.LevelKey, x=>x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
            monsters.TryGetValue(typeId, out MonsterStaticData staticData) 
                ? staticData 
                : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
    }
}