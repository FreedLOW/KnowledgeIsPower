using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId monsterTypeId;
        public bool slain;

        private IGameFactory gameFactory;
        private EnemyDeath enemyDeath;

        public string Id { get; set; }

        public void Construct(IGameFactory factory)
        {
            gameFactory = factory;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                slain = true;
            else Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (slain)
                progress.KillData.ClearedSpawners.Add(Id);
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