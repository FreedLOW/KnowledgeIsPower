using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
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

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressesWriters { get; } = new List<ISavedProgress>();
        
        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService)
        {
            this.assetProvider = assetProvider;
            this.staticData = staticData;
            this.randomService = randomService;
            this.progressService = progressService;
        }

        public GameObject CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegister(prefabPath: AssetsPath.HeroPath, at: at.transform.position);
            return HeroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegister(AssetsPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(progressService.PlayerProgress.WorldData);
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

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressesWriters.Clear();
        }

        public void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                RegisterProgressReader(progressReader);
            }
        }

        public void RegisterProgressReader(ISavedProgressReader progressReader)
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