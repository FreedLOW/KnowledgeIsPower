using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster", order = 51)]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        [Range(1, 30)] public float MovementSpeed = 3f;
        [Range(1, 100)] public float MaxHP;
        [Range(1, 30)] public float Damage;
        [Range(1, 10)] public float AttackCooldown = 3f;

        [Range(0, 100)] public int MinMoneyLoot;
        [Range(0, 100)] public int MaxMoneyLoot;

        [Range(0.5f, 1f)] public float AttackRadius;
        [Range(0.5f, 1f)] public float EffectiveDistance;

        public GameObject Prefab;
    }
}