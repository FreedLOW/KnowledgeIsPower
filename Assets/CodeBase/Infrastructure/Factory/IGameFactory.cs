using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressesWriters { get; }
        Task WarmUp();
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        Task<LootPiece> CreateLoot();
        Task CreateSpawner(Vector3 at, string spawnId, MonsterTypeId monsterTypeId);
        Task CreateLeftLoot(Loot leftLoot, string leftLootId);
        Task<LevelTransferTrigger> CreateLevelTransferTrigger(Vector3 at);
        void CleanUp();
    }
}