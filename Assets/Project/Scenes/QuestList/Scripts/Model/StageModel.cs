using System.Collections.Generic;
using Project.Commons.Scripts.Extensions;
using Project.Scripts.Model;
using Project.Scripts.Infra;
using UnityEngine;

namespace Project.Scenes.QuestList.Scripts.Model
{
    public class StageModel : ModelBase
    {
        List<StageData> stages;

        public StageModel(List<StageData> stages)
        {
            this.stages = stages;
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