using System;
using UniRx;
using UnityEngine;

namespace Project.Commons.BossPhaseStateMachine.Scripts.Model
{
    public class BossHealthModel
    {
        private float maxHp;
        private ReactiveProperty<float> currentHp = new (100);
        
        public float MaxHp => maxHp;
        public IReadOnlyReactiveProperty<float> CurrentHp => currentHp;
        
        public void TakeDamage(float damage)
        {
            currentHp.Value = Mathf.Max(0, currentHp.Value - damage);
        }
    }
}
