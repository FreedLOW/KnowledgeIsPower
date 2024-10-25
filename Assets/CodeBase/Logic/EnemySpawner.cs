using CodeBase.Data;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        
        private string _id;
        [SerializeField] private bool slain;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
            {
                slain = true;
            }
            else
            {
                Spawn();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (slain) 
                progress.KillData.ClearedSpawners.Add(_id);
        }

        private void Spawn()
        {
            
        }
    }
}