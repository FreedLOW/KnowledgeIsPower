﻿using System.Linq;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator enemyAnimator;

        public float attackCooldown = 3f;
        public float attackRadius;
        public float effectiveDistance;
        public float damageToHero;
        
        private Transform playerTransform;

        private float currentAttackCooldown;
        private bool isAttacking;
        private int playerLayerMask;
        private Collider[] hits = new Collider[1];  // since the intersection will only be with player, set the array size to 1
        private bool attackIsActive;

        public void Construct(Transform heroTransform)
        {
            playerTransform = heroTransform;
        }

        private void Awake()
        {
            playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();
            
            if (CanAttack())
                StartAttack();
        }

        private void OnDrawGizmos()
        {
            if (!Hit(out Collider hit)) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(AttackPointPosition(), attackRadius);
        }

        public void SetAttackData(MonsterStaticData monsterData)
        {
            attackRadius = monsterData.AttackRadius;
            effectiveDistance = monsterData.EffectiveDistance;
            damageToHero = monsterData.Damage;
            attackCooldown = monsterData.AttackCooldown;
        }

        /// <summary>
        /// Unity animation event callback
        /// </summary>
        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(AttackPointPosition(), attackRadius, 2f);
                hit.GetComponent<IHealth>().TakeDamage(damageToHero);
            }
        }

        /// <summary>
        /// Unity animation event callback
        /// </summary>
        private void OnFinishAttack()
        {
            currentAttackCooldown = attackCooldown;
            isAttacking = false;
        }

        public void EnableAttack() => 
            attackIsActive = true;

        public void DisableAttack() => 
            attackIsActive = false;

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(AttackPointPosition(), attackRadius, hits, playerLayerMask);
            hit = hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private Vector3 AttackPointPosition() =>
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
            transform.forward * effectiveDistance;

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                currentAttackCooldown -= Time.deltaTime;
        }

        private bool CanAttack() => 
            attackIsActive && CooldownIsUp() && !isAttacking;

        private bool CooldownIsUp() => 
            currentAttackCooldown <= 0f;

        private void StartAttack()
        {
            transform.LookAt(playerTransform);
            enemyAnimator.PlayAttack();
            isAttacking = true;
        }
    }
}