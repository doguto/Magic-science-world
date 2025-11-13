using System;
using Project.Commons.EnemyBulletPrototype.Scripts.Model;
using Project.Commons.EnemyBulletPrototype.Scripts.View;
using UniRx;
using UnityEngine;

namespace Project.Commons.EnemyBulletPrototype.Scripts.Presenter
{
    public class EnemyBulletPresenter : IDisposable
    {
        private BulletModel model;
        private EnemyBulletView view;
        private BulletManager bulletManager;
        
        // 購読を管理するCompositeDisposable
        private CompositeDisposable disposables = new CompositeDisposable();

        public void Init(BulletModel model, EnemyBulletView view, BulletManager bulletManager)
        {
            this.model = model;
            this.view = view;
            this.bulletManager = bulletManager;
            // Viewを初期化
            view.Init(model.Velocity, model.LifeTime);
            
            // Viewのイベントを購読
            view.OnHit
                .Subscribe(HandleHit)
                .AddTo(disposables);
            
            view.OnLifeTimeExpired
                .Subscribe(_ => HandleLifeTimeExpired())
                .AddTo(disposables);
        }
        
        private void HandleHit(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // プレイヤーに当たった
                HitPlayer(other);
            }
            else if (other.CompareTag("PlayerAttack"))
            {
                // プレイヤーの攻撃を受けた
                TakeDamageFromPlayer(other);
            }
        }
        
        /// <summary>
        /// プレイヤーに当たったときの処理
        /// </summary>
        private void HitPlayer(Collider2D playerCollider)
        {
            // TODO: プレイヤーにダメージを与える
            // var playerModel = playerCollider.GetComponent<PlayerView>().GetModel();
            // playerModel.TakeDamage(model.Damage);
            
            Debug.Log($"プレイヤーに {model.Damage} ダメージ");
            
            // 弾を破棄
            DestroyBullet();
        }
        
        /// <summary>
        /// プレイヤーの攻撃を受けたときの処理
        /// </summary>
        private void TakeDamageFromPlayer(Collider2D attackCollider)
        {
            // 攻撃からダメージ量を取得
            // TODO: AttackViewを実装したら差し替え
            int attackDamage = 1; // とりあえず固定値
            
            // Modelにダメージを与える
            bool isDestroyed = model.TakeDamage(attackDamage);
            
            Debug.Log($"弾が {attackDamage} ダメージを受けた（残りHP: {model.CurrentHp}）");
            
            if (isDestroyed)
            {
                Debug.Log("弾が破壊された");
                // TODO: 破壊エフェクト再生
                DestroyBullet();
            }
        }
        
        /// <summary>
        /// 寿命切れ処理
        /// </summary>
        private void HandleLifeTimeExpired()
        {
            Debug.Log("弾の寿命が切れた");
            DestroyBullet();
        }
        
        /// <summary>
        /// 弾を破棄してPoolに返却
        /// </summary>
        private void DestroyBullet()
        {
            // 購読を全てDispose
            Dispose();
            
            // Viewをクリーンアップ
            view.Cleanup();
            
            // Managerに返却を通知
            bulletManager.ReturnBullet(view, this);
        }
        
        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}