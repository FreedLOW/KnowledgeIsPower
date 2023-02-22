using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider assetProvider;
        private readonly IStaticDataService staticData;
        private readonly IRandomService randomService;
        private readonly IPersistentProgressService progressService;
        private readonly IWindowService windowService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressesWriters { get; } = new List<ISavedProgress>();
        
        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService, IWindowService windowService)
        {
            this.assetProvider = assetProvider;
            this.staticData = staticData;
            this.randomService = randomService;
            this.progressService = progressService;
            this.windowService = windowService;
        }

        public GameObject CreateHero(Vector3 at)
        {
            HeroGameObject = InstantiateRegister(prefabPath: AssetsPath.HeroPath, at);
            return HeroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegister(AssetsPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(progressService.PlayerProgress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Construct(windowService);
            }
            
            return hud;
        }

        public GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent)
        {
            MonsterStaticData monsterData = staticData.ForMonster(monsterTypeId);

            GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);
            
            var health = monster.GetComponent<IHealth>();
            health.CurrentHP = monsterData.MaxHP;
            health.MaxHP = monsterData.MaxHP;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<Follow>().Construct(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MovementSpeed;

            var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.MinMoneyLoot, monsterData.MaxMoneyLoot);
            lootSpawner.Construct(this, randomService);

            var attack = monster.GetComponent<Attack>();
            attack.Construct(HeroGameObject.transform);
            attack.attackRadius = monsterData.AttackRadius;
            attack.effectiveDistance = monsterData.EffectiveDistance;
            attack.damageToHero = monsterData.Damage;
            attack.attackCooldown = monsterData.AttackCooldown;
            
            return monster;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegister(AssetsPath.LootPath)
                .GetComponent<LootPiece>();
            lootPiece.Construct(progressService.PlayerProgress.WorldData);
            return lootPiece;
        }

        public void CreateSpawner(Vector3 at, string spawnId, MonsterTypeId monsterTypeId)
        {
            var spawner = InstantiateRegister(AssetsPath.SpawnPath, at).GetComponent<SpawnPoint>();

            spawner.Construct(this);
            spawner.monsterTypeId = monsterTypeId;
            spawner.Id = spawnId;
        }

        public void CreateLeftLoot(Loot leftLoot, string leftLootId)
        {
            LootPiece lootPiece = CreateLoot();
            lootPiece.transform.position = leftLoot.Position.AsUnityVector();
            lootPiece.SetId(leftLootId);
            lootPiece.Initialize(leftLoot);
            RegisterProgressReader(lootPiece);
        }

        public LevelTransferTrigger CreateLevelTransferTrigger(Vector3 at) =>
            InstantiateRegister(AssetsPath.LevelTransferPath, at)
                .GetComponent<LevelTransferTrigger>();

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressesWriters.Clear();
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                RegisterProgressReader(progressReader);
            }
        }

        private void RegisterProgressReader(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressesWriters.Add(progressWriter);
            
            ProgressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegister(string prefabPath, Vector3 at)
        {
            var gameObject = assetProvider.Instantiate(prefabPath, at: at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegister(string prefabPath)
        {
            var gameObject = assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
    }
}