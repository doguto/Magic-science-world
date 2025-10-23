using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Model;
using Project.Scripts.Infra;
using Project.Scripts.Repository.AssetRepository;
using UnityEngine;

namespace Project.Scenes.StageList.Scripts.Model
{
    public class StageModel : ModelBase
    {
        List<StageData> stages;
        List<Sprite> charaImages = new();

        public StageModel(List<StageData> stages)
        {
            this.stages = stages;

            var stillAssetRepository = new StillAssetRepository();
            charaImages = stages
                .Select(x => stillAssetRepository.Load(x.charaStillAddress, false))
                .ToList();
        }
        
        public Sprite GetCharaImage(int stageNumber)
        {
            // stageNumberは1からなので1引いて合わせる
            var sprite = charaImages[stageNumber - 1];
            return sprite;
        }
    }
}
