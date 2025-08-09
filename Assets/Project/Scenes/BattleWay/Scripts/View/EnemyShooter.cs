// Assets/Project/Scenes/BattleWay/Scripts/View/EnemyShooter.cs
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Project.Scenes.BattleWay.Scripts.View
{
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.BulletPatterns;
    using Project.Scenes.BattleWay.Scripts.View.Pool;

    /// <summary>
    /// 弾発射装置。View層：Transform操作のみを担当。
    /// Presenterから Setup(...) で起動される想定。
    /// </summary>
    public class EnemyShooter : MonoBehaviour
    {
        [Header("任意：発射位置（未指定なら自身のTransform）")]
        [SerializeField] private Transform _firePoint;

        private BulletPatternSO _pattern;
        private BulletPool _pool;
        private CancellationTokenSource _cts;

        public bool IsRunning { get; private set; }

        /// <summary>
        /// 発射初期化。以降 fireInterval ごとに発射。
        /// </summary>
        public void Setup(BulletPatternSO pattern, BulletPool pool, Transform firePoint = null)
        {
            _pattern = pattern;
            _pool = pool;
            if (firePoint != null) _firePoint = firePoint;

            RestartLoop().Forget();
        }

        private async UniTaskVoid RestartLoop()
        {
            StopLoop();
            _cts = new CancellationTokenSource();

            if (_pattern == null)
            {
                Debug.LogWarning("[EnemyShooter] Pattern 未設定です。");
                return;
            }
            if (_pool == null)
            {
                Debug.LogWarning("[EnemyShooter] BulletPool 未設定です。");
                return;
            }

            IsRunning = true;
            var token = _cts.Token;

            // 無限ループ（Destroy/Disableでキャンセル）
            while (!token.IsCancellationRequested)
            {
                FireOnce();
                var intervalMs = Mathf.Max(1, (int)(_pattern.fireInterval * 1000f));
                try
                {
                    await UniTask.Delay(intervalMs, cancellationToken: token);
                }
                catch (System.OperationCanceledException) { break; }
            }

            IsRunning = false;
        }

        private void FireOnce()
        {
            var baseAngle = _pattern.baseAngleDeg;
            var count = Mathf.Max(1, _pattern.count);
            var arc = _pattern.arcAngleDeg;

            // 角度配列生成
            switch (_pattern.spreadType)
            {
                case BulletSpreadType.Single:
                {
                    SpawnAtAngle(baseAngle);
                    break;
                }
                case BulletSpreadType.Even:
                {
                    if (count == 1)
                    {
                        SpawnAtAngle(baseAngle);
                    }
                    else
                    {
                        float start = baseAngle - arc * 0.5f;
                        float step = arc / (count - 1);
                        for (int i = 0; i < count; i++)
                            SpawnAtAngle(start + step * i);
                    }
                    break;
                }
                case BulletSpreadType.RandomInArc:
                {
                    float start = baseAngle - arc * 0.5f;
                    for (int i = 0; i < count; i++)
                    {
                        float a = start + Random.value * arc;
                        SpawnAtAngle(a);
                    }
                    break;
                }
            }
        }

        private void SpawnAtAngle(float angleDeg)
        {
            var fp = _firePoint != null ? _firePoint : transform;
            var dir = new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
            var bullet = _pool.Rent(fp.position, Quaternion.identity);
            bullet.Init(dir, _pattern.bulletSpeed, _pattern.bulletLifetime);
        }

        private void OnDisable() => StopLoop();
        private void OnDestroy() => StopLoop();

        private void StopLoop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
            IsRunning = false;
        }
    }
}
