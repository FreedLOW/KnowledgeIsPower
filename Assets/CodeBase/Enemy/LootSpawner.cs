using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath enemyDeath;
        
        private IGameFactory gameFactory;
        private IRandomService randomService;

        private int lootMin;
        private int lootMax;

        public void Construct(IGameFactory gameFactory, IRandomService randomService)
        {
            this.gameFactory = gameFactory;
            this.randomService = randomService;
        }
        
        private void Start()
        {
            enemyDeath.OnEnemyDeath += SpawnLoot;
        }

        private void SpawnLoot()
        {
            enemyDeath.OnEnemyDeath -= SpawnLoot;

            LootPiece loot = gameFactory.CreateLoot();
            loot.transform.position = transform.position;

            Loot lootItem = GenerateLootItem();
            loot.Initialize(lootItem);
        }

        public void SetLoot(int min, int max)
        {
            lootMin = min;
            lootMax = max;
        }

        private Loot GenerateLootItem()
        {
            return new Loot
            {
                MoneyValue = randomService.Next(lootMin, lootMax),
                Position = transform.position.AsVectorData()
            };
        }
    }
}