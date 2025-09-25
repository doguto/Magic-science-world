using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Commons.DataBase.Scripts;
using Project.Commons.Scripts.Extensions;
using Project.Commons.Scripts.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scenes.QuestList.Scripts.Model
{
    public class QuestModel : ModelBase
    {
        List<StageData> stages;

        public QuestModel()
        {
            stages = LoadData<StageDataObject>("StageData").stageData;
        }
        
        public Sprite GetCharaImage(int stageNumber)
        {
            // stageNumberは1からなので1引いて合わせる
            var stageData = stages[stageNumber - 1];
            if (stageData.charaStillAddress.IsNullOrEmpty()) return null;

            var sprite = LoadAsset<Sprite>(stageData.charaStillAddress);
            return sprite;
        }
    }
}