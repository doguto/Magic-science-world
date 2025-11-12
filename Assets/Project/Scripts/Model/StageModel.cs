using Project.Scripts.Infra;
using Project.Scripts.Repository.AssetRepository;
using UnityEngine;

namespace Project.Scripts.Model
{
    public class StageModel : ModelBase
    {
        public StageData StageData { get; }

        public Sprite CharaImage { get; }
        public bool IsCleared { get; private set; }

        
        public StageModel(StageData stageData)
        {
            StageData = stageData;

            var stillAssetRepository = new StillAssetRepository();
            CharaImage = stillAssetRepository.Load(StageData.charaStillAddress, false);
        }

        public void Clear()
        {
            IsCleared = true;
        }
    }
}
