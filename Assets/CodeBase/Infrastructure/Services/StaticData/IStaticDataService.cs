using CodeBase.StaticData;
using CodeBase.StaticData.Window;
using CodeBase.UI.Services.Window;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadMonsters();
        void LoadLevels();
        void LoadWindowConfigs();
        MonsterStaticData GetMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string levelKey);
        WindowConfig ForWindow(WindowId windowId);
    }
}