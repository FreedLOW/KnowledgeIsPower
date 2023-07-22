using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Code
{
    public class PlayerProgressDataTest
    {
        private static PlayerProgress NewProgress() => new PlayerProgress(initialLevel: "Main");

        [Test]
        public void WhenChangeHeroHealth_AndHeroHealthIs50_ThenHeroHealthShouldBe20()
        {
            // Arrange.
            IPersistentProgressService progress = Substitute.For<IPersistentProgressService>();
            progress.PlayerProgress = NewProgress();
            progress.PlayerProgress.HeroState.CurrentHP = 50;

            // Act.
            progress.PlayerProgress.HeroState.CurrentHP -= 30;

            // Assert.
            progress.PlayerProgress.HeroState.CurrentHP.Should().Be(20);
        }

        [Test]
        public void WhenAddLootAmount_AndLootCollectedIs0_ThenCollectedShouldBe30()
        {
            // Arrange.
            IPersistentProgressService progress = Substitute.For<IPersistentProgressService>();
            progress.PlayerProgress = NewProgress();
            
            // Act.
            progress.PlayerProgress.WorldData.LootData.Add(30);
            
            // Assert.
            progress.PlayerProgress.WorldData.LootData.Collected.Should().Be(30);
        }

        [Test]
        public void WhenChangeHeroDamageStat_AndHeroDamageStatIs0_ThenDamageStatShouldBe15()
        {
            // Arrange.
            IPersistentProgressService progress = Substitute.For<IPersistentProgressService>();
            progress.PlayerProgress = NewProgress();
            
            // Act.
            progress.PlayerProgress.HeroStats.Damage += 15;
            
            // Assert.
            progress.PlayerProgress.HeroStats.Damage.Should().Be(15);
        }
    }
}