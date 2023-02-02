using CodeBase.Data;
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

        private void Awake()
        {
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
            
        }
    }
}