using System;

namespace Project.Scenes.BattleWay.Scripts.Model
{
    using Project.Scenes.BattleWay.Scripts.ScriptableObjects.MovePatterns;

    /// <summary>
    /// 敵の状態と設定値の保持（MonoBehaviour 非依存）
    /// </summary>
    public class EnemyModel : IDisposable
    {
        public EnemyMovePatternSO MovePattern { get; private set; }

        public EnemyModel(EnemyMovePatternSO pattern)
        {
            MovePattern = pattern;
        }

        public void SetPattern(EnemyMovePatternSO pattern)
        {
            MovePattern = pattern;
        }

        public void Dispose()
        {
            // 現時点では特に解放対象なし
        }
    }
}