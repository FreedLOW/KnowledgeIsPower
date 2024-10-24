using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        public HeroAnimator Animator;
        public CharacterController CharacterController;

        private readonly Collider[] _hits = new Collider[3];
        
        private int _hittableLayer;
        private HeroStats _heroStats;

        private IInputService _inputService;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();

            _hittableLayer = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (CanAttack()) 
                Animator.PlayAttack();
        }

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                _hits[i].transform.parent.GetComponent<IHealth>()
                    .TakeDamage(_heroStats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress) => 
            _heroStats = progress.HeroStats;

        private bool CanAttack() => 
            _inputService.IsAttackButtonUp() && !Animator.IsAttacking;

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(AttackPoint() + transform.forward, _heroStats.RadiusDamage, _hits, _hittableLayer);

        private Vector3 AttackPoint() => 
            new(transform.position.x, CharacterController.center.y / 2f, transform.position.z);
    }
}