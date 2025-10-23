using System.Collections.Generic;
using Project.Scenes.StageList.Scripts.Model;
using Project.Scripts.Infra;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Repository.ModelRepository
{
    public class StageModelRepository : ModelRepositoryBase
    {
        public static StageModelRepository Instance { get; } = new();
        
        private List<StageData> stages;
        private StageModel stageModel;

        public StageModelRepository()
        {
            dataName = "StageData";
            stages = LoadData();
        }

        // Get()はModel毎に処理が変わる。
        // 引数でint idを取ったりするものもあるかも
        public StageModel Get()
        {
            if (stageModel != null) return stageModel;
            stageModel = new StageModel(stages);
            return stageModel;
        }

        List<StageData> LoadData()
        {
            var dataObject = Addressables.LoadAssetAsync<StageDataObject>(DataAddress).WaitForCompletion();
            return dataObject.stageData;
        }
    }
}
