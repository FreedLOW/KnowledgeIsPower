using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        public CharacterController characterController;
        public HeroAnimator heroAnimator;

        private IInputService inputService;

        private static int hitMask;
        private Collider[] hits = new Collider[4];  // how many enemies can take damage when attack
        private Stats heroStats;

        private void Awake()
        {
            inputService = AllServices.Container.Single<IInputService>();

            hitMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (inputService.IsAttackButtonUp() && !heroAnimator.IsAttacking)
                heroAnimator.PlayAttack();
        }

         /// <summary>
        /// Unity animation event callback 
        /// </summary>
        private void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(heroStats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress) => 
             heroStats = progress.HeroStats;

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, 
                                          heroStats.AttackRadius, hits, hitMask);

        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, characterController.center.y / 2, transform.position.z);
    }
}