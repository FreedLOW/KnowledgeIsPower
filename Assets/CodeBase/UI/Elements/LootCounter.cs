using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LootCounter : MonoBehaviour
    {
        public TextMeshProUGUI LootText;
        
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.LootData.OnCollected += OnCollected;
            
            OnCollected();
        }

        private void OnDestroy()
        {
            if (_worldData != null) 
                _worldData.LootData.OnCollected -= OnCollected;
        }

        private void OnCollected()
        {
            LootText.text = $"{_worldData.LootData.Collected}";
        }
    }
}