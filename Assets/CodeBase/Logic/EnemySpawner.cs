using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId monsterTypeId;
        public bool slain;
        
        private string id;
        private IGameFactory gameFactory;
        private EnemyDeath enemyDeath;

        private void Awake()
        {
            gameFactory = AllServices.Container.Single<IGameFactory>();
            id = GetComponent<UniqueId>().uniqueId;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(id))
                slain = true;
            else Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (slain)
                progress.KillData.ClearedSpawners.Add(id);
        }

        private void Spawn()
        {
            GameObject monster = gameFactory.CreateMonster(monsterTypeId, transform);
            enemyDeath = monster.GetComponent<EnemyDeath>();
            enemyDeath.OnEnemyDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath()
        {
            if (enemyDeath != null)
                enemyDeath.OnEnemyDeath -= OnEnemyDeath;
            slain = true;
        }
    }
}