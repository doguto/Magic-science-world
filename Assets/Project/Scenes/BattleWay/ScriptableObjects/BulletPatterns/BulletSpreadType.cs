// Assets/Project/Scenes/BattleWay/Scripts/ScriptableObjects/BulletPatterns/BulletSpreadType.cs
namespace Project.Scenes.BattleWay.Scripts.ScriptableObjects.BulletPatterns
{
    /// <summary>
    /// 弾の拡散方式
    /// </summary>
    public enum BulletSpreadType
    {
        Single,     // 単発
        Even,       // 均等分割（扇状）
        RandomInArc // 扇範囲内でランダム
    }
}
