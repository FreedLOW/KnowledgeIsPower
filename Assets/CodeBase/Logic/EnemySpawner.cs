using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        
        private string _id;
        [SerializeField] private bool _slain;
        private GameObject _monster;
        
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
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
                progress.KillData.ClearedSpawners.Add(_id);
        }

        private void Spawn()
        {
            _monster = _gameFactory.CreateMonster(MonsterTypeId, transform);
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