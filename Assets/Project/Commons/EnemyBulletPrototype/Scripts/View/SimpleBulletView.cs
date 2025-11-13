using UnityEngine;

namespace Project.Commons.EnemyBulletPrototype.Scripts.View
{
    public class SimpleBulletView : EnemyBulletView
    {
        protected override void UpdateMovement()
        {
            // 単純に速度ベクトル方向に移動
            transform.position += Velocity * Time.deltaTime;
        }

    }
}