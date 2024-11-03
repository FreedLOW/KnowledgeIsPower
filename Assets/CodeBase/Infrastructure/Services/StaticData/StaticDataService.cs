using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Window;
using CodeBase.UI.Services.Window;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters = new();
        private Dictionary<string, LevelStaticData> _levels = new();
        private Dictionary<WindowId, WindowConfig> _windowConfigs = new();

        public void LoadMonsters() =>
            _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(t => t.MonsterTypeId, t => t);

        public void LoadLevels() =>
            _levels = Resources.LoadAll<LevelStaticData>("StaticData/Levels")
                .ToDictionary(t => t.LevelKey, t => t);
        
        public void LoadWindowConfigs() =>
            _windowConfigs = Resources.Load<WindowStaticData>("StaticData/UI/WindowData")
                .Configs
                .ToDictionary(t => t.WindowId, t => t);

        public MonsterStaticData GetMonster(MonsterTypeId typeId) => 
            _monsters.GetValueOrDefault(typeId, null);

        public LevelStaticData ForLevel(string levelKey) => 
            _levels.GetValueOrDefault(levelKey, null);

        public WindowConfig ForWindow(WindowId windowId) => 
            _windowConfigs.GetValueOrDefault(windowId, null);
    }
}