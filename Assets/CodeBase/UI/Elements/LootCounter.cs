using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LootCounter : MonoBehaviour
    {
        public TextMeshProUGUI CounterText;
        private WorldData worldData;

        public void Construct(WorldData worldData)
        {
            this.worldData = worldData;
            this.worldData.LootData.Changed += UpdateCounter;
            
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            CounterText.text = $"{worldData.LootData.Collected}";
        }
    }
}