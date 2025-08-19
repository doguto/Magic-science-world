using UnityEngine;
using System.Collections.Generic;

namespace Project.Scenes.BattleWay.Scripts.Presenter
{
    using Project.Commons.DataBase.ScriptableObjects;
    using Project.Scenes.BattleWay.Scripts.View;

    /// <summary>
    /// 敵の種類とPrefabの対応を管理するレジストリ
    /// Presenter層でView依存を管理する
    /// </summary>
    [CreateAssetMenu(
        fileName = "EnemyRegistry", 
        menuName = "BattleWay/EnemyRegistry", 
        order = 2)]
    public class EnemyRegistry : ScriptableObject
    {
        [System.Serializable]
        public class EnemyEntry
        {
            [Tooltip("敵の種類")]
            public EnemyType enemyType;
            
            [Tooltip("対応するPrefab")]
            public EnemyView enemyPrefab;
            
            [Tooltip("デフォルトの敵データ")]
            public EnemyDataSO defaultEnemyData;
        }

        [Header("敵の種類とPrefabの対応")]
        public List<EnemyEntry> enemyEntries = new List<EnemyEntry>();

        /// <summary>
        /// 敵の種類からPrefabを取得
        /// </summary>
        public EnemyView GetEnemyPrefab(EnemyType enemyType)
        {
            var entry = enemyEntries.Find(e => e.enemyType == enemyType);
            return entry?.enemyPrefab;
        }

        /// <summary>
        /// 敵の種類からデフォルトデータを取得
        /// </summary>
        public EnemyDataSO GetDefaultEnemyData(EnemyType enemyType)
        {
            var entry = enemyEntries.Find(e => e.enemyType == enemyType);
            return entry?.defaultEnemyData;
        }

        /// <summary>
        /// 有効なエントリかチェック
        /// </summary>
        public bool IsValidEntry(EnemyType enemyType)
        {
            return GetEnemyPrefab(enemyType) != null;
        }
    }
}
