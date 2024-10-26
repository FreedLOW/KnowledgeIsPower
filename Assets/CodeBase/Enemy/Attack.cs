using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;

        public float Cooldown = 3f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 0.5f;
        public float Damage = 10f;

        private readonly Collider[] _hits = new Collider[1];

        private Transform _heroTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _heroLayer;
        private bool _isAttackActive;

        public void Construct(Transform heroTransform)
        {
            _heroTransform = heroTransform;
        }

        private void Awake()
        {
            _heroLayer = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(HitPoint(), Cleavage, 2f);
                hit.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }

        private void OnAttackEnded()
        {
            _attackCooldown = Cooldown;
            _isAttacking = false;
        }

        public void EnableAttack() => 
            _isAttackActive = true;

        public void DisableAttack() => 
            _isAttackActive = false;

        private bool Hit(out Collider hit)
        {
            var hitCount = Physics.OverlapSphereNonAlloc(HitPoint(), Cleavage, _hits, _heroLayer);
            hit = _hits[0];
            return hitCount > 0;
        }

        private Vector3 HitPoint() => 
            transform.position + new Vector3(0f, 0.5f, 0f) + transform.forward * EffectiveDistance;

        private bool CanAttack() => 
            _isAttackActive && !_isAttacking && CooldownIsUp();

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            Animator.PlayAttack();
            _isAttacking = true;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;
        }

        private bool CooldownIsUp() => 
            _attackCooldown <= 0;
    }
}