// Assets/Project/Scenes/BattleWay/Scripts/ScriptableObjects/Waves/SpawnEntry.cs
using System;
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.ScriptableObjects.Waves
{
    using Project.Scenes.BattleWay.Scripts.View;
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.MovePatterns;
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.BulletPatterns;

    [Serializable]
    public class SpawnEntry
    {
        [Tooltip("この時刻(秒)になったらスポーン")]
        public float spawnTime = 0f;

        [Tooltip("出現させる敵Prefab（EnemyViewを持つこと）")]
        public EnemyView enemyPrefab;

        [Tooltip("初期位置（ワールド座標）")]
        public Vector2 spawnPosition = Vector2.zero;

        [Tooltip("移動パターン")]
        public EnemyMovePatternSO movePattern;

        [Tooltip("弾発射パターン（未指定で射撃なし）")]
        public BulletPatternSO bulletPattern;
    }
}
