// Assets/Project/Scenes/BattleWay/Scripts/Presenter/WaveRunner.cs
using System.Collections.Generic;

namespace Project.Scenes.BattleWay.Scripts.Presenter
{
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.Waves;

    /// <summary>
    /// 経過時間に応じて SpawnEntry を吐き出す小クラス（ピュア）
    /// Presenter から Update(deltaTime) で呼ばれ、到達分を返す。
    /// </summary>
    public class WaveRunner
    {
        private readonly List<SpawnEntry> _entries;
        private int _cursor;
        private float _elapsed;

        public bool Finished => _entries == null || _cursor >= _entries.Count;

        public WaveRunner(WaveSO wave)
        {
            _entries = wave?.entries ?? new List<SpawnEntry>();
            _entries.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));
            _cursor = 0;
            _elapsed = 0f;
        }

        /// <summary>
        /// 経過時間を進め、到達したエントリをまとめて返す
        /// </summary>
        public List<SpawnEntry> Update(float deltaTime)
        {
            _elapsed += deltaTime;
            var spawned = new List<SpawnEntry>();

            while (_cursor < _entries.Count)
            {
                var e = _entries[_cursor];
                if (_elapsed + 1e-4f < e.spawnTime) break; // まだ時間に満たない

                spawned.Add(e);
                _cursor++;
            }

            return spawned;
        }

        public void Reset()
        {
            _cursor = 0;
            _elapsed = 0f;
        }
    }
}
