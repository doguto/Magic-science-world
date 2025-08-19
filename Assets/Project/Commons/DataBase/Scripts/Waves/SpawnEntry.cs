// Assets/Project/Commons/DataBase/ScriptableObjects/Waves/SpawnEntry.cs
using System;
using UnityEngine;

namespace Project.Commons.DataBase.Scripts
{
    [Serializable]
    public class SpawnEntry
    {
        [Tooltip("この時刻(秒)になったらスポーン")]
        public float spawnTime = 0f;

        [Tooltip("出現させる敵の種類")]
        public EnemyType enemyType = EnemyType.BasicEnemy;

        [Tooltip("敵のデータ（任意：未設定時はenemyTypeから自動推定）")]
        public EnemyDataSO enemyData;

        [Tooltip("初期位置（ワールド座標）")]
        public Vector2 spawnPosition = Vector2.zero;

        [Tooltip("移動パターン")]
        public EnemyMovePatternSO movePattern;

        [Tooltip("弾発射パターン（未指定で射撃なし）")]
        public BulletPatternSO bulletPattern;
    }
}
