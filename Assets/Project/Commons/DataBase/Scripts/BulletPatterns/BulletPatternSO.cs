// Assets/Project/Scenes/BattleWay/Scripts/ScriptableObjects/BulletPatterns/BulletPatternSO.cs
using UnityEngine;

namespace Project.Commons.DataBase.Scripts
{
    /// <summary>
    /// 弾発射パターン（第5回でEnemyShooterから使用）
    /// </summary>
    [CreateAssetMenu(fileName = "BulletPattern", menuName = "BattleWay/BulletPattern", order = 0)]
    public class BulletPatternSO : ScriptableObject
    {
        [Header("発射パラメータ")]
        [Tooltip("弾速（ユニット/秒）")]
        public float bulletSpeed = 6f;

        [Tooltip("発射間隔（秒）")]
        public float fireInterval = 0.25f;

        [Tooltip("一度に発射する弾の数")]
        public int count = 1;

        [Tooltip("拡散方式")]
        public BulletSpreadType spreadType = BulletSpreadType.Single;

        [Tooltip("中心角（度）。発射の基準となる角度（0=右、90=上）")]
        public float baseAngleDeg = 270f;

        [Tooltip("扇の総角度（度）。Even/RandomInArcで使用")]
        public float arcAngleDeg = 45f;

        [Header("寿命")]
        [Tooltip("弾の生存時間（秒）。0以下で無制限")]
        public float bulletLifetime = 6f;
    }
}
