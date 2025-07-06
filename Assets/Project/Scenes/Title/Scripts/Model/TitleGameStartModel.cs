using Project.Commons.DataBase.Scripts;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.Model
{
    public class TitleGameStartModel
    {
        readonly GameData gameData;
        
        public TitleGameStartModel()
        {
            gameData = Resources.Load<GameData>("ProjectData/GameData");
        }

        public void StartGame()
        {
            // クリアステージを初期化
            gameData.ClearedStageNumber = 0;
        }
    }
}