using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Loots;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;

        private int _lootMin;
        private int _lootMax;

        private IGameFactory _factory;
        private IRandomService _random;

        public void Construct(IGameFactory factory, IRandomService randomService)
        {
            _factory = factory;
            _random = randomService;
        }

        private void Start()
        {
            EnemyDeath.OnDeath += OnSpawnLoot;
        }

        private void OnDestroy()
        {
            EnemyDeath.OnDeath -= OnSpawnLoot;
        }

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }

        private async void OnSpawnLoot()
        {
            LootPiece loot = await _factory.SpawnLoot();
            loot.transform.position = transform.position;

            Loot spawnLoot = GenerateLoot();
            loot.Initialize(spawnLoot);
        }

        private Loot GenerateLoot() =>
            new()
            {
                Value = _random.Next(_lootMin, _lootMax)
            };
    }
}