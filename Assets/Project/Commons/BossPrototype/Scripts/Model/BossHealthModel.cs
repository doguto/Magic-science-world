using System;
using UniRx;
using UnityEngine;

namespace Project.Commons.BossPrototype.Scripts.Model
{
    public class BossHealthModel: IDisposable
    {
        public float MaxHp { get; }
        private ReactiveProperty<float> currentHp = new (100);
        public IReadOnlyReactiveProperty<float> CurrentHp => currentHp;

        public BossHealthModel(float maxHp)
        {
            MaxHp = maxHp;
            currentHp.Value = maxHp;
        }
        public void TakeDamage(float damage)
        {
            currentHp.Value = Mathf.Max(0, currentHp.Value - damage);
        }
        
        public bool IsDead => currentHp.Value <= 0;
        
        public void Dispose()
        {
            currentHp.Dispose();
        }
    }
}
