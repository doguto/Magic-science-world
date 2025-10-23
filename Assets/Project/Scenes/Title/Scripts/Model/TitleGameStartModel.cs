using Project.Scripts.Infra;
using Project.Scripts.Model;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.Model
{
    public class TitleGameStartModel : ModelBase
    {
        readonly UserDataObject userDataObject = new();

        public void StartGame()
        {
            // クリアステージを初期化
            userDataObject.ClearedStageNumber = 0;
        }
    }
}
