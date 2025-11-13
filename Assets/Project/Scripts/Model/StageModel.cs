using System;
using Project.Scripts.Infra;
using Project.Scripts.Repository.AssetRepository;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Project.Scripts.Model
{
    public class StageModel : ModelBase
    {
        public StageData StageData { get; }

        public Sprite CharaImage { get; }
        public bool IsOpend { get; private set; }
        public bool IsCleared { get; private set; }


        public StageModel(StageData stageData, bool isOpened = false, bool isCleared = false)
        {
            StageData = stageData;
            IsOpend = isOpened;
            IsCleared = isCleared && isOpened; // 念のため isOpened と AND する

            var stillAssetRepository = new StillAssetRepository();
            CharaImage = stillAssetRepository.Load(StageData.charaStillAddress, false);
        }

        public void Open()
        {
            IsOpend = true;
        }

        public void Clear()
        {
            if (!IsOpend) throw new Exception("ステージが開放されていません.");

            IsCleared = true;
        }
    }
}
