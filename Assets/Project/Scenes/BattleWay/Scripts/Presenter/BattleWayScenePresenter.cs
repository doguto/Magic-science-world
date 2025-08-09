// Assets/Project/Scenes/BattleWay/Scripts/Presenter/BattleWayScenePresenter.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.Presenter
{
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.Waves;
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.BulletPatterns;
    using Project.Scenes.BattleWay.Scripts.View;
    using Project.Scenes.BattleWay.Scripts.View.Pool;

    /// <summary>
    /// BattleWay シーンの司令塔。
    /// - WaveSO を時間再生して敵をスポーン
    /// - MovePattern を EnemyView へ適用
    /// - BulletPattern があれば Shooter をセットアップ
    /// -（第6回）HitDispatcher を購読し、とりあえずログ
    /// </summary>
    public class BattleWayScenePresenter : MonoBehaviour
    {
        [Header("進行データ")]
        [SerializeField] private WaveSO _wave;

        [Header("生成先")]
        [SerializeField] private Transform _enemiesRoot;

        [Header("弾プール（射撃するなら必須）")]
        [SerializeField] private BulletPool _bulletPool;

        private WaveRunner _waveRunner;
        private readonly List<EnemyView> _spawned = new();

        // （R6）Hit購読の解除用
        private readonly List<(HitDispatcher disp, Action<HitDispatcher.HitEvent> handler)> _hitSubscriptions = new();

        private void Start()
        {
            if (_wave == null)
            {
                Debug.LogError("[BattleWayScenePresenter] WaveSO が未設定です。");
                return;
            }

            _waveRunner = new WaveRunner(_wave);
        }

        private void Update()
        {
            if (_waveRunner == null || _waveRunner.Finished) return;

            var due = _waveRunner.Update(Time.deltaTime);
            if (due == null || due.Count == 0) return;

            foreach (var e in due)
            {
                if (e == null || e.enemyPrefab == null)
                {
                    Debug.LogWarning("[BattleWayScenePresenter] SpawnEntry が不正（Prefab未設定など）");
                    continue;
                }

                // 生成
                var enemy = Instantiate(e.enemyPrefab, (Vector3)e.spawnPosition, Quaternion.identity, _enemiesRoot);

                // 移動初期化 & 開始
                if (e.movePattern != null)
                {
                    enemy.Init(e.movePattern, e.spawnPosition);
                }
                enemy.ApplyMove();

                // 射撃セットアップ（パターン指定があれば）
                if (e.bulletPattern != null && _bulletPool != null)
                {
                    enemy.SetupShooter(e.bulletPattern, _bulletPool, null);
                }

                // ヒットイベント購読（R6：ログのみ）
                var disp = enemy.GetComponentInChildren<HitDispatcher>(true);
                if (disp != null)
                {
                    Action<HitDispatcher.HitEvent> h = (ev) =>
                    {
                        Debug.Log($"[Hit] {ev.selfTag}({ev.self.name}) x {ev.otherTag}({ev.other.name})");
                    };
                    disp.OnHit += h;
                    _hitSubscriptions.Add((disp, h));
                }

                _spawned.Add(enemy);
            }
        }

        private void OnDestroy()
        {
            foreach (var (disp, handler) in _hitSubscriptions)
            {
                if (disp != null) disp.OnHit -= handler;
            }
            _hitSubscriptions.Clear();
        }
    }
}
