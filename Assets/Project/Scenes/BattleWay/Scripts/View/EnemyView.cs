// Assets/Project/Scenes/BattleWay/Scripts/View/EnemyView.cs
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Project.Scenes.BattleWay.Scripts.View
{
    using Project.Scenes.BattleWay.Scripts.Model;
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.MovePatterns;
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.BulletPatterns;
    using Project.Scenes.BattleWay.Scripts.View.Pool;

    /// <summary>
    /// Transform への移動適用（DOTween 専任）＋ Shooter の薄い窓口。
    /// Presenter から Init / ApplyMove / SetupShooter を呼ばれる想定。
    /// 単体検証用途として、_movePattern を Inspector で与えれば自動起動にも対応。
    /// </summary>
    public class EnemyView : MonoBehaviour
    {
        [Header("（単体検証用）Presenter未使用でも動かす場合に設定")]
        [SerializeField] private EnemyMovePatternSO _movePattern;

        [Header("Shooter 連携（任意）")]
        [SerializeField] private EnemyShooter _shooter;
        [SerializeField, Tooltip("未指定なら EnemyShooter 側の設定を使用")]
        private Transform _firePointForShooter;

        private EnemyModel _model;
        private Tween _runningTween;
        private bool _initialized;   // Presenter から Init 済みか

        /// <summary>Presenter から初期化される想定。</summary>
        public void Init(EnemyMovePatternSO pattern)
        {
            _model?.Dispose();
            _model = new EnemyModel(pattern);
            _initialized = true;
        }

        /// <summary>位置も同時に初期化できるオーバーロード。</summary>
        public void Init(EnemyMovePatternSO pattern, Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            Init(pattern);
        }

        /// <summary>現在位置を基準に移動 Tween を適用。</summary>
        public void ApplyMove()
        {
            var pattern = _model != null ? _model.MovePattern : _movePattern;
            if (pattern == null)
            {
                Debug.LogWarning("[EnemyView] MovePattern が未設定です。");
                return;
            }

            KillTweenIfAny();

            switch (pattern.moveType)
            {
                case MoveType.Straight:
                    _runningTween = ApplyStraight(pattern);
                    break;
                case MoveType.Zigzag:
                    _runningTween = ApplyZigzag(pattern);
                    break;
                case MoveType.SineWave:
                    _runningTween = ApplySineWave(pattern);
                    break;
                case MoveType.Circle:
                    _runningTween = ApplyCircle(pattern);
                    break;
            }

            if (pattern.loop && _runningTween != null)
            {
                _runningTween.SetLoops(-1, pattern.loopType);
            }
        }

        /// <summary>Shooter を Presenter からセットアップするための薄い窓口。</summary>
        public void SetupShooter(BulletPatternSO pattern, BulletPool pool, Transform firePointOverride = null)
        {
            if (pattern == null || pool == null) return;

            if (_shooter == null)
            {
                _shooter = GetComponentInChildren<EnemyShooter>(true);
                if (_shooter == null)
                {
                    Debug.LogWarning("[EnemyView] EnemyShooter が見つかりません。スキップします。");
                    return;
                }
            }

            var fp = firePointOverride != null ? firePointOverride : _firePointForShooter;
            _shooter.Setup(pattern, pool, fp);
        }

        private void Start()
        {
            // ★ 単体検証用の自動起動：
            // Presenter から Init 済みなら何もしない。未初期化＋_movePattern があれば自動で走らせる。
            if (!_initialized && _movePattern != null)
            {
                Init(_movePattern);
                ApplyMove();
            }
        }

        private void OnDestroy()
        {
            KillTweenIfAny();
            _model?.Dispose();
        }

        private void KillTweenIfAny()
        {
            if (_runningTween != null && _runningTween.IsActive())
            {
                _runningTween.Kill();
                _runningTween = null;
            }
        }

        // ==============================
        // DOTween 実装
        // ==============================

        private Tween ApplyStraight(EnemyMovePatternSO pattern)
        {
            var waypoints = BuildWaypointsFromOffsets(pattern);
            if (waypoints.Count == 0) return null;

            float duration = CalcDuration(pattern, waypoints);
            return transform
                .DOPath(waypoints.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.clear)
                .SetEase(pattern.ease);
        }

        private Tween ApplyZigzag(EnemyMovePatternSO pattern)
        {
            var (start, end) = GetStartEnd(pattern);
            int n = Mathf.Max(8, pattern.samples);
            var pts = new List<Vector3>(n + 1);

            for (int i = 0; i <= n; i++)
            {
                float t = (float)i / n;
                var p = Vector3.Lerp(start, end, t);

                float sign = (i % 2 == 0) ? 1f : -1f;
                Vector3 dir = (end - start).normalized;
                Vector3 right = new Vector3(-dir.y, dir.x, 0f); // 進行直交
                p += right * pattern.zigzagAmplitude * sign;

                pts.Add(p);
            }

            float duration = CalcDuration(pattern, pts);
            return transform
                .DOPath(pts.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.clear)
                .SetEase(pattern.ease);
        }

        private Tween ApplySineWave(EnemyMovePatternSO pattern)
        {
            var (start, end) = GetStartEnd(pattern);
            int n = Mathf.Max(16, pattern.samples);
            var pts = new List<Vector3>(n + 1);

            Vector3 dir = (end - start).normalized;
            Vector3 right = new Vector3(-dir.y, dir.x, 0f);

            float totalDist = Vector3.Distance(start, end);

            for (int i = 0; i <= n; i++)
            {
                float t = (float)i / n;
                var baseP = Vector3.Lerp(start, end, t);
                float phase = t * totalDist * pattern.sineFrequency; // 距離ベース周波数
                float offset = Mathf.Sin(phase * Mathf.PI * 2f) * pattern.sineAmplitude;
                baseP += right * offset;
                pts.Add(baseP);
            }

            float duration = CalcDuration(pattern, pts);
            return transform
                .DOPath(pts.ToArray(), duration, PathType.CatmullRom, PathMode.Full3D, 10, Color.clear)
                .SetEase(pattern.ease);
        }

        private Tween ApplyCircle(EnemyMovePatternSO pattern)
        {
            Vector3 center = transform.position + (Vector3)pattern.centerOffset;
            int n = Mathf.Max(16, pattern.samples);
            var pts = new List<Vector3>(n + 1);

            float dir = Mathf.Sign(pattern.circleDirection) == 0 ? 1f : Mathf.Sign(pattern.circleDirection);
            for (int i = 0; i <= n; i++)
            {
                float t = (float)i / n;
                float ang = dir * t * Mathf.PI * 2f;
                var p = center + new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0f) * pattern.circleRadius;
                pts.Add(p);
            }

            float duration = Mathf.Max(0.01f, pattern.circlePeriod);
            return transform
                .DOPath(pts.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.clear)
                .SetEase(Ease.Linear);
        }

        // ==============================
        // 補助
        // ==============================

        private (Vector3 start, Vector3 end) GetStartEnd(EnemyMovePatternSO pattern)
        {
            Vector3 start = transform.position;
            Vector3 end = start + (pattern.pathPoints != null && pattern.pathPoints.Length > 0
                ? (Vector3)pattern.pathPoints[pattern.pathPoints.Length - 1]
                : new Vector3(0f, -5f, 0f));
            return (start, end);
        }

        private List<Vector3> BuildWaypointsFromOffsets(EnemyMovePatternSO pattern)
        {
            var result = new List<Vector3>();
            if (pattern.pathPoints == null || pattern.pathPoints.Length == 0)
            {
                result.Add(transform.position + new Vector3(0f, -5f, 0f));
                return result;
            }

            Vector3 basePos = transform.position;
            foreach (var off in pattern.pathPoints)
            {
                result.Add(basePos + (Vector3)off);
            }
            return result;
        }

        private float CalcDuration(EnemyMovePatternSO pattern, List<Vector3> points)
        {
            if (points == null || points.Count == 0) return Mathf.Max(0.01f, pattern.duration);

            float dist = 0f;
            for (int i = 0; i < points.Count; i++)
            {
                dist += Vector3.Distance(i == 0 ? transform.position : points[i - 1], points[i]);
            }

            if (pattern.speed > 0.0001f)
            {
                return Mathf.Max(0.01f, dist / pattern.speed);
            }
            return Mathf.Max(0.01f, pattern.duration);
        }
    }
}
