using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;

        private bool _slain;
        private GameObject _monster;

        public string Id { get; set; }

        private IGameFactory _gameFactory;

        public void Construct(IGameFactory factory)
        {
            _gameFactory = factory;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
            {
                _slain = true;
            }
            else
            {
                Spawn();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain) 
                progress.KillData.ClearedSpawners.Add(Id);
        }

        private async void Spawn()
        {
            _monster = await _gameFactory.CreateMonster(MonsterTypeId, transform);
            _monster.GetComponent<EnemyDeath>().OnDeath += OnDeath;
        }

        private void OnDeath()
        {
            if (_monster != null)
                _monster.GetComponent<EnemyDeath>().OnDeath -= OnDeath;

            _slain = true;
        }
    }
}