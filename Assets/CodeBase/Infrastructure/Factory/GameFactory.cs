using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task WarmUp()
        {
            await assetProvider.Load<GameObject>(AssetsAddress.Loot);
            await assetProvider.Load<GameObject>(AssetsAddress.Spawn);
        }

        public async Task<GameObject> CreateHero(Vector3 at)
        {
            HeroGameObject = await InstantiateRegisterAsync(prefabPath: AssetsAddress.Hero, at);
            return HeroGameObject;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisterAsync(AssetsAddress.Hud);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(progressService.PlayerProgress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Construct(windowService);
            }
            
            return hud;
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent)
        {
            MonsterStaticData monsterData = staticData.ForMonster(monsterTypeId);

            GameObject prefab = await assetProvider.Load<GameObject>(monsterData.PrefabReference);
            GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
            
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
            attack.SetAttackData(monsterData);

            return monster;
        }

        public async Task<LootPiece> CreateLoot()
        {
            var prefab = await assetProvider.Load<GameObject>(AssetsAddress.Loot);
            var lootPiece = InstantiateRegisterAsync(prefab)
                .GetComponent<LootPiece>();
            lootPiece.Construct(progressService.PlayerProgress.WorldData);
            return lootPiece;
        }

        public async Task CreateSpawner(Vector3 at, string spawnId, MonsterTypeId monsterTypeId)
        {
            GameObject prefab = await assetProvider.Load<GameObject>(AssetsAddress.Spawn);
            var spawner = InstantiateRegisterAsync(prefab, at).GetComponent<SpawnPoint>();

            spawner.Construct(this);
            spawner.monsterTypeId = monsterTypeId;
            spawner.Id = spawnId;
        }

        public async Task CreateLeftLoot(Loot leftLoot, string leftLootId)
        {
            LootPiece lootPiece = await CreateLoot();
            lootPiece.transform.position = leftLoot.Position.AsUnityVector();
            lootPiece.SetId(leftLootId);
            lootPiece.Initialize(leftLoot);
            RegisterProgressReader(lootPiece);
        }

        public async Task<LevelTransferTrigger> CreateLevelTransferTrigger(Vector3 at)
        {
            GameObject prefab = await InstantiateRegisterAsync(AssetsAddress.LevelTransfer, at);
            var levelTransferTrigger = prefab.GetComponent<LevelTransferTrigger>();
            return levelTransferTrigger;
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressesWriters.Clear();
            assetProvider.CleanUp();
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

        private GameObject InstantiateRegisterAsync(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegisterAsync(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisterAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await assetProvider.Instantiate(prefabPath, at: at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisterAsync(string prefabPath)
        {
            GameObject gameObject = await assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
    }
}