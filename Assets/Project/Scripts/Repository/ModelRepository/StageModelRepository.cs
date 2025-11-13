using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Model;
using Project.Scripts.Infra;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Repository.ModelRepository
{
    public class StageModelRepository : ModelRepositoryBase
    {
        public static StageModelRepository Instance { get; } = new();

        readonly List<StageData> stageData;
        readonly List<StageModel> stageModels = new();

        public StageModelRepository()
        {
            dataName = "StageData";
            stageData = LoadData();
            foreach (var data in stageData)
            {
                // TODO: UserModelが実装出来次第、UserModelのステージ進捗度からIsCleared等を取得して渡す
                stageModels.Add(new StageModel(data));
            }
        }

        public StageModel Get(int stageId)
        {
            var index = stageId - 1;
            if (index < 0 || stageModels.Count <= index) return null;
            if (stageModels.Count <= index) return stageModels[index];

            for (var i = stageModels.Count; i < stageData.Count; i++)
            {
                stageModels.Add(new StageModel(stageData[i]));
            }

            return stageModels[index];
        }

        public List<StageModel> All() => stageModels;

        public void Clear()
        {
            stageModels.Clear();
        }
        
        List<StageData> LoadData()
        {
            var dataObject = Addressables.LoadAssetAsync<StageDataObject>(DataAddress).WaitForCompletion();
            return dataObject.stageData;
        }
    }
}
