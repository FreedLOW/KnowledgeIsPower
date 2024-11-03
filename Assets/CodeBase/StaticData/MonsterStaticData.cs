using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        [Range(1, 50)]
        public int HP;

        [Range(1f, 30f)]
        public float Damage;
        
        [Range(0.5f, 1f)]
        public float AttackCleavage;
        
        [Range(0.5f, 3f)]
        public float AttackEffectiveDistance;

        [Range(0.5f, 10f)] 
        public float MoveSpeed;

        [Range(1, 5)] 
        public int LootMin;
        [Range(5, 10)] 
        public int LootMax;
        
        public GameObject Prefab;
    }
}