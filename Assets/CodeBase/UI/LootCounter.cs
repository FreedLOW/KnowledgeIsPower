using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class LootCounter : MonoBehaviour, ISavedProgressReader
    {
        public TextMeshProUGUI CounterText;
        private WorldData worldData;

        public void Construct(WorldData worldData)
        {
            this.worldData = worldData;
            this.worldData.LootData.Changed += UpdateCounter;
        }

        private void Start()
        {
            UpdateCounter();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            worldData.LootData.Collected = progress.WorldData.LootData.Collected;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            CounterText.text = $"{worldData.LootData.Collected}";
        }
    }
}