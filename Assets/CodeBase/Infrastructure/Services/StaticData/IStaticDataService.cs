using CodeBase.StaticData;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadMonsters();
        void LoadLevels();
        MonsterStaticData GetMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string levelKey);
    }
}