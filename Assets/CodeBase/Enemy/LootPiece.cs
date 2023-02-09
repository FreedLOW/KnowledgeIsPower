using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        public GameObject LootObject;
        public GameObject PickupFxPrefab;
        public GameObject PickupPopup;
        public TextMeshPro LootText;
        
        private Loot loot;
        private WorldData worldData;
        private PlayerProgress playerProgress;
        private bool isPicked;
        private string id;

        public void Construct(WorldData worldData)
        {
            this.worldData = worldData;
        }

        public void Initialize(Loot loot)
        {
            this.loot = loot;
        }

        public void SetId(string lootId)
        {
            id = lootId;
        }

        private void OnTriggerEnter(Collider other)
        {
            Pickup();
        }

        private void Pickup()
        {
            if (isPicked) return;
            
            isPicked = true;

            UpdateWorldData();
            HideLootObject();
            PlayPickupFx();
            ShowLootText();
            StartCoroutine(DestroyTimer());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            SetProgress(progress);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            SetProgress(progress);
            
            if (isPicked || IsLootExist()) return;

            SetId(GetComponent<UniqueId>().uniqueId);
            progress.LeftLoot.IdLeftLoots.Add(id);
            progress.LeftLoot.Loots.Add(loot);
        }

        private void SetProgress(PlayerProgress progress)
        {
            if (playerProgress == null)
                playerProgress = progress;
        }

        private void UpdateWorldData() => 
            worldData.LootData.Collect(loot);

        private void HideLootObject() => 
            LootObject.SetActive(false);

        private void PlayPickupFx() => 
            Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowLootText()
        {
            LootText.text = $"{loot.MoneyValue}";
            PickupPopup.SetActive(true);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            RemoveSavedLootData();
            Destroy(gameObject);
        }

        private void RemoveSavedLootData()
        {
            if (playerProgress == null) return;
            
            if (IsLootExist())
            {
                playerProgress.LeftLoot.IdLeftLoots.Remove(id);
                playerProgress.LeftLoot.Loots.Remove(loot);
            }
        }

        private bool IsLootExist() =>
            playerProgress.LeftLoot.IdLeftLoots.Count > 0 &&
            playerProgress.LeftLoot.IdLeftLoots.Contains(id);
    }
}