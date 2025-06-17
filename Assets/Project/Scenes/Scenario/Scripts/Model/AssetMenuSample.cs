using UnityEngine;

namespace Project.Scenes.Scenario.Scripts.Model.AssetMenuSample
{
    [CreateAssetMenu(
        fileName = "NewEnemyData",
        menuName = "Scenario/EnemyData",
        order = 0)]
    public class EnemyData : ScriptableObject
    {
        public string enemyName;
        public int maxHP;
        public float moveSpeed;
    }
}
