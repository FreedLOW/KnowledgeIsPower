using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.PersistentProgress;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Code
{
    public class HealthTest
    {
        private static PlayerProgress NewProgress() => new PlayerProgress(initialLevel: "Main");

        [Test]
        public void WhenDamageHero_AndHeroHealthIs100_ThenHeroHealthShouldBe50()
        {
            // Arrange.
            IPersistentProgressService progress = Substitute.For<IPersistentProgressService>();
            progress.PlayerProgress = NewProgress();
            var hero = new GameObject();
            var health = hero.AddComponent<HeroHealth>();
            var heroAnimator = hero.AddComponent<HeroAnimator>();
            heroAnimator.Animator = hero.AddComponent<Animator>();
            health.heroAnimator = heroAnimator;
            health.LoadProgress(progress.PlayerProgress);
            
            health.CurrentHP = 100;

            // Act.
            health.TakeDamage(50);

            // Assert.
            health.CurrentHP.Should().Be(50);
        }

        [Test]
        public void WhenDamageEnemy_AndEnemyHealthIs100_ThenEnemyHealthShouldBe70()
        {
            // Arrange.
            var enemy = new GameObject();
            EnemyHealth health = enemy.AddComponent<EnemyHealth>();
            var enemyAnimator = enemy.AddComponent<EnemyAnimator>();
            enemyAnimator.animator = enemy.AddComponent<Animator>();
            health.enemyAnimator = enemyAnimator;
            
            health.CurrentHP = 100;

            // Act.
            health.TakeDamage(30);

            // Assert.
            health.CurrentHP.Should().Be(70);
        }
    }
}