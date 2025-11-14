using UniRx;
using UnityEngine;

namespace Project.Commons.EnemyBulletPrototype.Scripts.Model
{
    [System.Serializable]
    public class BulletModel
    {
        public float Damage { get; private set; }
        public int MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float LifeTime { get; private set; }
    
        public BulletModel(float damage, int maxHp, Vector2 velocity, float lifeTime)
        {
            Damage = damage;
            MaxHp = maxHp;
            CurrentHp = maxHp;
            Velocity = velocity;
            LifeTime = lifeTime;
        }
        
        private bool IsDead => CurrentHp <= 0;
        
        public bool TakeDamage(float damage)
        {
            CurrentHp = Mathf.Max(0, CurrentHp - damage);
            return IsDead;
        }
    }
}