using UnityEngine;
using UniRx;
using System;

namespace Project.Commons.EnemyBulletPrototype.Scripts.View
{
    public abstract class EnemyBulletView : MonoBehaviour
    {
        protected Vector3 Velocity;
        protected float remainingLifeTime;
        
        // Presenterに通知するイベント
        private Subject<Collider2D> onHitSubject = new();
        private Subject<Unit> onLifeTimeExpiredSubject = new();

        public IObservable<Collider2D> OnHit => onHitSubject;
        public IObservable<Unit> OnLifeTimeExpired => onLifeTimeExpiredSubject;
        
        /// <summary>
        /// 初期化（Presenterから呼ばれる）
        /// </summary>
        public void Init(Vector3 velocity, float lifeTime)
        {
            onHitSubject.Dispose();
            onHitSubject = new();
            
            this.Velocity = velocity;
            this.remainingLifeTime = lifeTime;
        }
        
        void Update()
        {
            // Time.timeScale = 0 のときは動かさない（ポーズ対応）
            if (Time.timeScale == 0) return;
            
            // 継承先で実装された動き方
            UpdateMovement();
            
            // 寿命管理
            remainingLifeTime -= Time.deltaTime;
            if (remainingLifeTime <= 0)
            {
                onLifeTimeExpiredSubject?.OnNext(Unit.Default);
            }
        }
        
        /// <summary>
        /// 動き方の実装（継承先で定義）
        /// </summary>
        protected abstract void UpdateMovement();
        
        void OnTriggerEnter2D(Collider2D other)
        {
            // Presenterに当たり判定を通知
            onHitSubject.OnNext(other);
        }
        
        public virtual void Cleanup()
        {
            onHitSubject.Dispose();
            onLifeTimeExpiredSubject.Dispose();
        }
        void OnDestroy()
        {
            Cleanup();
        }
    }
}