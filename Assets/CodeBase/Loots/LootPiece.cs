using System.Collections;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Logic;
using TMPro;
using UnityEngine;

namespace CodeBase.Loots
{
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        public GameObject Skull;
        public GameObject PickupFxPrefab;
        public TextMeshPro LootText;
        public GameObject PickupPopup;
        
        private bool _picked;
        private WorldData _worldData;
        private Loot _loot;
        private string _id;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other)
        {
            Pickup();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_picked)
                return;

            TrySetId();
            progress.WorldData.LootData.AddLeftLoot(_id, transform.position.AsVectorData(), _loot);
        }

        public void SetWorldPosition(Vector3Data position) => 
            transform.position = position.AsUnityVector();

        public void SetId(string leftLootId) => 
            _id = leftLootId;

        private void TrySetId()
        {
            if (string.IsNullOrWhiteSpace(_id)) 
                _id = GetComponent<UniqueId>().Id;
        }

        private void Pickup()
        {
            if (_picked)
                return;

            _picked = true;
            UpdateWorldData();
            HideSkull();
            PlayPickupFx();
            ShowLootText();
            StartCoroutine(DestroyTimerRoutine());
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSkull() => 
            Skull.SetActive(false);

        private void PlayPickupFx() => 
            Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowLootText()
        {
            LootText.text = _loot.Value.ToString();
            PickupPopup.SetActive(true);
        }

        private void RemoveLeftLoot()
        {
            _worldData.LootData.RemoveLeftLoot(_id);
        }

        private IEnumerator DestroyTimerRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            
            RemoveLeftLoot();
            Destroy(gameObject);
        }
    }
}