using UnityEngine;

namespace Project.Commons.DataBase.Scripts
{
    /// <summary>
    /// 敵の基本データを定義するScriptableObject
    /// Viewに依存しない純粋なデータ定義
    /// </summary>
    [CreateAssetMenu(
        fileName = "EnemyData", 
        menuName = "BattleWay/EnemyData", 
        order = 1)]
    public class EnemyDataSO : ScriptableObject
    {
        [Header("基本情報")]
        [Tooltip("敵の種類")]
        public EnemyType enemyType = EnemyType.BasicEnemy;
        
        [Tooltip("敵の名前")]
        public string enemyName = "基本敵";
        
        [Tooltip("説明")]
        [TextArea(2, 4)]
        public string description = "";
        
        [Header("ステータス")]
        [Tooltip("体力")]
        public int hitPoints = 100;
        
        [Tooltip("攻撃力")]
        public int attackPower = 10;
        
        [Tooltip("移動速度倍率")]
        public float speedMultiplier = 1.0f;
        
        [Header("視覚的要素")]
        [Tooltip("敵のスプライト（アイコンなど）")]
        public Sprite enemySprite;
        
        [Tooltip("敵の色")]
        public Color enemyColor = Color.white;
    }
}
