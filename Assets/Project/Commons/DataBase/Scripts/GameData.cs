using Project.Commons.Scripts.Extensions;
using UnityEditor;
using UnityEngine;

namespace Project.Commons.DataBase.Scripts
{
    public class GameData : ScriptableObject
    {
        [ReadOnly]
        public int clearedStageNumber { get; private set; }
    }

    public static class CreateEnemyParamDataAssetFromCsv
    {
        const string AssetPath = "Assets/Resources/ProjectData/";

        [MenuItem("ScriptableObjects/CreateGameData")]
        public static void CreateEnemyParamDataAsset()
        {
            var gameData = ScriptableObject.CreateInstance<GameData>();
        
            var assetName = $"{AssetPath}GameData.asset";
            AssetDatabase.CreateAsset(gameData, assetName);

            // Asset作成後、反映させるために必要なメソッド
            AssetDatabase.Refresh();
        }
    }
}

