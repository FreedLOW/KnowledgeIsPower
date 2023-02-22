using System.Collections.Generic;
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
        GameObject CreateHero(Vector3 at);
        GameObject CreateHud();
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
        void CreateSpawner(Vector3 at, string spawnId, MonsterTypeId monsterTypeId);
        void CreateLeftLoot(Loot leftLoot, string leftLootId);
        LevelTransferTrigger CreateLevelTransferTrigger(Vector3 at);
        void Cleanup();
    }
}