﻿using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.Loots;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progress;

        private GameObject _hero;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressesWriters { get; } = new();

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData, IRandomService randomService,
            IPersistentProgressService progressService)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _randomService = randomService;
            _progress = progressService;
        }

        public GameObject CreateHero(GameObject at)
        {
            _hero = InstantiateRegistered(AssetsPath.HeroPath, at: at.transform.position);
            return _hero;
        }

        public GameObject CreateHud()
        {
            var hud = InstantiateRegistered(AssetsPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progress.PlayerProgress.WorldData);
            
            return hud;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressesWriters.Clear();
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressesWriters.Add(progressWriter);
            
            ProgressReaders.Add(progressReader);
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            var monsterData = _staticData.GetMonster(typeId);
            var monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

            monster.GetComponent<Follow>().Construct(_hero.transform);

            var monsterHealth = monster.GetComponent<IHealth>();
            monsterHealth.MaxHP = monsterData.HP;
            monsterHealth.CurrentHP = monsterData.HP;
            
            monster.GetComponent<ActorUI>().Construct(monsterHealth);

            var attack = monster.GetComponent<Attack>();
            attack.Construct(_hero.transform);
            attack.Damage = monsterData.Damage;
            attack.EffectiveDistance = monsterData.AttackEffectiveDistance;
            attack.Cleavage = monsterData.AttackCleavage;

            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLoot(monsterData.LootMin, monsterData.LootMax);

            return monster;
        }

        public LootPiece SpawnLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetsPath.Loot).GetComponent<LootPiece>();
            lootPiece.Construct(_progress.PlayerProgress.WorldData);
            return lootPiece;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assetProvider.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }
    }
}