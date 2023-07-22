using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services.PersistentProgress;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace Tests.Code
{
    public class AddLootTest
    {
        private static PlayerProgress NewProgress() => new PlayerProgress(initialLevel: "Main");

        private static Loot GenerateLootItem()
        {
            return new Loot
            {
                MoneyValue = 1,
                Position = default
            };
        }

        [Test]
        public void WhenStoringSkull_AndSkullCountIs0_ThenCountShouldBe1()
        {
            // Arrange.
            var progress = Substitute.For<IPersistentProgressService>();
            progress.PlayerProgress = NewProgress();
            var loot = GenerateLootItem();

            var lootPiece = new GameObject().AddComponent<LootPiece>();
            var lootObject = new GameObject();
            var pickUpFx = new GameObject();
            var lootText = new GameObject().AddComponent<TextMeshPro>();
            var pickUpPopUp = new GameObject();

            lootPiece.LootObject = lootObject;
            lootPiece.PickupFxPrefab = pickUpFx;
            lootPiece.LootText = lootText;
            lootPiece.PickupPopup = pickUpPopUp;
            lootPiece.Construct(progress.PlayerProgress.WorldData);
            lootPiece.Initialize(loot);

            // Act.
            lootPiece.Pickup();
            var result = progress.PlayerProgress.WorldData.LootData.Collected;

            // Assert.
            result.Should().Be(1);
        }
    }
}