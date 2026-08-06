using UnityEngine;

namespace Project.Scenes.Battle.Scripts.Model.Entity
{
    public class EnemyEntityModel : EntityBase
    {
        public EnemyEntityModel(int maxHp, int contactDamage, bool onlyChargeDamageable = false)
            : base(maxHp)
        {
            ContactDamage = contactDamage;
            OnlyChargeDamageable = onlyChargeDamageable;
        }

        public override bool IsPlayer => false;

        public int ContactDamage { get; }

        // 通常攻撃では被ダメージ0、チャージ攻撃のみ被ダメージする敵か
        public bool OnlyChargeDamageable { get; }

        public override void OnCollision(EntityBase other)
        {
            if (other is BulletEntityModel bullet)
            {
                // 通常攻撃無効の敵に通常弾が当たった場合は被ダメージ0（＝ダメージ処理をスキップ）
                if (OnlyChargeDamageable && !bullet.IsPlayerChargeBullet)
                    return;

                TakeDamage(bullet.Damage);
            }
        }

        protected override void OnDeathCore()
        {
            Debug.Log("[EnemyEntityModel] Enemy died.");
        }
    }
}
