using UnityEngine;
using DG.Tweening;

namespace Project.Commons.DataBase.Scripts
{
    /// <summary>
    /// 敵の移動パターン定義（Presenterは値を読むだけ、ViewがTransformに適用）
    /// </summary>
    [CreateAssetMenu(
        fileName = "EnemyMovePattern",
        menuName = "BattleWay/MovePattern",
        order = 0)]
    public class EnemyMovePatternSO : ScriptableObject
    {
        [Header("共通")]
        public MoveType moveType = MoveType.Straight;

        [Tooltip("速度（>0 なら距離/速度で所要時間を自動計算）。0 なら duration を使用")]
        public float speed = 0f;

        [Tooltip("明示的に動作時間を指定（speed=0 のとき有効）")]
        public float duration = 2f;

        [Tooltip("移動の経路点（ワールド座標の相対オフセット）。Straight/波形の進行方向の目安にも使う")]
        public Vector2[] pathPoints = new Vector2[] { new Vector2(0, -5f) };

        [Header("補間")]
        public Ease ease = Ease.Linear;
        public bool loop = false;
        public LoopType loopType = LoopType.Restart;

        [Header("Sine / Zigzag 用（左右振幅や周波数）")]
        public float sineAmplitude = 1.5f;
        public float sineFrequency = 2f;

        public float zigzagAmplitude = 1.5f;
        public float zigzagFrequency = 3f;

        [Header("Circle 用")]
        [Tooltip("円運動の半径")]
        public float circleRadius = 2f;

        [Tooltip("円の中心オフセット（現在位置 + centerOffset が中心）")]
        public Vector2 centerOffset = Vector2.zero;

        [Tooltip("円運動の周回時間（秒）。speed は無視")]
        public float circlePeriod = 3f;

        [Tooltip("反時計回りなら +1、時計回りなら -1")]
        public int circleDirection = 1;

        /// <summary>
        /// 経路サンプル数（波形・円運動の分解能）
        /// </summary>
        [Range(8, 128)]
        public int samples = 32;
    }
}