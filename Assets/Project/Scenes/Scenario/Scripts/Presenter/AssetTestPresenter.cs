using UnityEngine;
using Project.Scenes.Scenario.Scripts.Model.AssetMenuSample;

namespace Project.Scenes.Scenario.Scripts.Presenter
{
    public class AssetTestPresenter : MonoBehaviour
    {
        [SerializeField] EnemyData enemyData;

        void Start()
        {
            Debug.Log($"Enemy: {enemyData.enemyName}, HP={enemyData.maxHP}, Speed={enemyData.moveSpeed}");
        }
        void Update()
        {
            Debug.Log($"Enemy: {enemyData.enemyName}, HP={enemyData.maxHP}, Speed={enemyData.moveSpeed}");
        }
    }
}
