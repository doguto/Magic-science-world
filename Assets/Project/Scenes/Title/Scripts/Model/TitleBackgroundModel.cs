using UnityEngine;
using Project.Scripts.Infra;


namespace Project.Scenes.Title.Scripts.Model
{
    public class TitleBackgroundModel
    {
        GameData _gameData;

        public int ClearedStageAmount => _gameData.ClearedStageNumber;
        
        public TitleBackgroundModel()
        {
            _gameData = Resources.Load<GameData>("ProjectData/GameData");
        }
    }
}