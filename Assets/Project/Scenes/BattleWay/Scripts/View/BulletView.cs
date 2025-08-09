// Assets/Project/Scenes/BattleWay/Scripts/View/BulletView.cs
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.View
{
    /// <summary>
    /// 単純な直進弾。Init で方向・速度・寿命を設定する。
    /// </summary>
    public class BulletView : MonoBehaviour
    {
        private Vector3 _dir;        // 正規化済みの移動方向
        private float _speed;        // 単位: ユニット/秒
        private float _lifetime;     // 残り寿命（秒）。0以下なら無制限
        private bool _initialized;

        /// <summary>
        /// 弾の初期化
        /// </summary>
        /// <param name="direction">ワールド方向ベクトル（正規化は内部で行う）</param>
        /// <param name="speed">ユニット/秒</param>
        /// <param name="lifetime">秒（0以下で無制限）</param>
        public void Init(Vector2 direction, float speed, float lifetime)
        {
            _dir = ((Vector3)direction).normalized;
            if (_dir.sqrMagnitude < 1e-6f) _dir = Vector3.down; // デフォルト下方向

            _speed = Mathf.Max(0f, speed);
            _lifetime = lifetime;
            _initialized = true;
        }

        private void OnEnable()
        {
            // プールから復帰したときのために寿命はInitで都度設定される前提
        }

        private void Update()
        {
            if (!_initialized) return;

            // 移動
            transform.position += _dir * (_speed * Time.deltaTime);

            // 寿命
            if (_lifetime > 0f)
            {
                _lifetime -= Time.deltaTime;
                if (_lifetime <= 0f)
                {
                    DespawnOrDestroy();
                }
            }
        }

        /// <summary>
        /// プールが存在すれば返却、無ければ破棄
        /// </summary>
        public void DespawnOrDestroy()
        {
            // ひとまず第4回では Destroy。第5回以降、プール連携で差し替え。
            Destroy(gameObject);
        }
    }
}
