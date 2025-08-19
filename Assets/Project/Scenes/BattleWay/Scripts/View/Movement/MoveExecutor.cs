// Assets/Project/Scenes/BattleWay/Scripts/View/Movement/MoveExecutor.cs
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Project.Scenes.BattleWay.Scripts.View.Movement
{
    using Project.Commons.DataBase.Scripts;

    /// <summary>
    /// 敵の移動実行を担当する専用クラス。
    /// DOTweenによる移動制御を単一責務として扱う。
    /// </summary>
    public class MoveExecutor
    {
        private readonly Transform _transform;
        private Tween _currentTween;

        public bool IsMoving => _currentTween != null && _currentTween.IsActive();

        public MoveExecutor(Transform transform)
        {
            _transform = transform;
        }

        /// <summary>
        /// 移動パターンを適用して移動を開始
        /// </summary>
        public void ExecuteMove(EnemyMovePatternSO pattern)
        {
            if (pattern == null)
            {
                Debug.LogWarning("[MoveExecutor] MovePattern が未設定です。");
                return;
            }

            StopCurrentMove();

            _currentTween = pattern.moveType switch
            {
                MoveType.Straight => CreateStraightTween(pattern),
                MoveType.Zigzag => CreateZigzagTween(pattern),
                MoveType.SineWave => CreateSineWaveTween(pattern),
                MoveType.Circle => CreateCircleTween(pattern),
                _ => null
            };

            if (_currentTween != null && pattern.loop)
            {
                _currentTween.SetLoops(-1, pattern.loopType);
            }
        }

        /// <summary>
        /// 現在の移動を停止
        /// </summary>
        public void StopCurrentMove()
        {
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
                _currentTween = null;
            }
        }

        /// <summary>
        /// リソースのクリーンアップ
        /// </summary>
        public void Dispose()
        {
            StopCurrentMove();
        }

        #region Private Move Creation Methods

        private Tween CreateStraightTween(EnemyMovePatternSO pattern)
        {
            var waypoints = BuildWaypointsFromOffsets(pattern);
            if (waypoints.Count == 0) return null;

            float duration = CalculateDuration(pattern, waypoints);
            return _transform
                .DOPath(waypoints.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.clear)
                .SetEase(pattern.ease);
        }

        private Tween CreateZigzagTween(EnemyMovePatternSO pattern)
        {
            var (start, end) = GetStartEndPositions(pattern);
            int sampleCount = Mathf.Max(8, pattern.samples);
            var points = new List<Vector3>(sampleCount + 1);

            for (int i = 0; i <= sampleCount; i++)
            {
                float t = (float)i / sampleCount;
                var basePoint = Vector3.Lerp(start, end, t);

                float sign = (i % 2 == 0) ? 1f : -1f;
                Vector3 direction = (end - start).normalized;
                Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f);
                basePoint += perpendicular * pattern.zigzagAmplitude * sign;

                points.Add(basePoint);
            }

            float duration = CalculateDuration(pattern, points);
            return _transform
                .DOPath(points.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.clear)
                .SetEase(pattern.ease);
        }

        private Tween CreateSineWaveTween(EnemyMovePatternSO pattern)
        {
            var (start, end) = GetStartEndPositions(pattern);
            int sampleCount = Mathf.Max(16, pattern.samples);
            var points = new List<Vector3>(sampleCount + 1);

            Vector3 direction = (end - start).normalized;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f);
            float totalDistance = Vector3.Distance(start, end);

            for (int i = 0; i <= sampleCount; i++)
            {
                float t = (float)i / sampleCount;
                var basePoint = Vector3.Lerp(start, end, t);
                float phase = t * totalDistance * pattern.sineFrequency;
                float offset = Mathf.Sin(phase * Mathf.PI * 2f) * pattern.sineAmplitude;
                basePoint += perpendicular * offset;
                points.Add(basePoint);
            }

            float duration = CalculateDuration(pattern, points);
            return _transform
                .DOPath(points.ToArray(), duration, PathType.CatmullRom, PathMode.Full3D, 10, Color.clear)
                .SetEase(pattern.ease);
        }

        private Tween CreateCircleTween(EnemyMovePatternSO pattern)
        {
            Vector3 center = _transform.position + (Vector3)pattern.centerOffset;
            int sampleCount = Mathf.Max(16, pattern.samples);
            var points = new List<Vector3>(sampleCount + 1);

            float direction = Mathf.Sign(pattern.circleDirection) == 0 ? 1f : Mathf.Sign(pattern.circleDirection);
            for (int i = 0; i <= sampleCount; i++)
            {
                float t = (float)i / sampleCount;
                float angle = direction * t * Mathf.PI * 2f;
                var point = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * pattern.circleRadius;
                points.Add(point);
            }

            float duration = Mathf.Max(0.01f, pattern.circlePeriod);
            return _transform
                .DOPath(points.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.clear)
                .SetEase(Ease.Linear);
        }

        #endregion

        #region Helper Methods

        private (Vector3 start, Vector3 end) GetStartEndPositions(EnemyMovePatternSO pattern)
        {
            Vector3 start = _transform.position;
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
                result.Add(_transform.position + new Vector3(0f, -5f, 0f));
                return result;
            }

            Vector3 basePosition = _transform.position;
            foreach (var offset in pattern.pathPoints)
            {
                result.Add(basePosition + (Vector3)offset);
            }
            return result;
        }

        private float CalculateDuration(EnemyMovePatternSO pattern, List<Vector3> points)
        {
            if (points == null || points.Count == 0) 
                return Mathf.Max(0.01f, pattern.duration);

            float totalDistance = 0f;
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 previous = i == 0 ? _transform.position : points[i - 1];
                totalDistance += Vector3.Distance(previous, points[i]);
            }

            if (pattern.speed > 0.0001f)
            {
                return Mathf.Max(0.01f, totalDistance / pattern.speed);
            }
            return Mathf.Max(0.01f, pattern.duration);
        }

        #endregion
    }
}
