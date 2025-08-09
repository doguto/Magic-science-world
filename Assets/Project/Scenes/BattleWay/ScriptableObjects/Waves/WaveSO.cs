// Assets/Project/Scenes/BattleWay/Scripts/ScriptableObjects/Waves/WaveSO.cs
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.ScriptableObjects.Waves
{
    /// <summary>
    /// 1ウェーブ分のスポーン指示セット
    /// </summary>
    [CreateAssetMenu(fileName = "Wave", menuName = "BattleWay/Wave", order = 0)]
    public class WaveSO : ScriptableObject
    {
        [Tooltip("spawnTime 昇順推奨")]
        public List<SpawnEntry> entries = new();

        [Tooltip("自動でspawnTime昇順に並べ替える（エディタ用）")]
        public bool sortOnValidate = true;

        private void OnValidate()
        {
            if (!sortOnValidate || entries == null) return;
            entries.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));
        }
    }
}
