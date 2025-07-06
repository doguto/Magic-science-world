using System;
using UnityEngine;
using Project.Commons.DataBase.Scripts;


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