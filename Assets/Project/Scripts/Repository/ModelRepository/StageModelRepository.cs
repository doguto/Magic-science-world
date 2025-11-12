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
        
        readonly List<StageData> stages;
        readonly List<StageModel> stageModels = new();

        public StageModelRepository()
        {
            dataName = "StageData";
            stages = LoadData();
            foreach (var stage in stages)
            {
                stageModels.Add(new StageModel(stage));
            }
        }

        public StageModel Get(int index)
        {
            if (stageModels.Count >= index) return null;
            
            return stageModels[index];
        }

        public List<StageModel> All() => stageModels;

        List<StageData> LoadData()
        {
            var dataObject = Addressables.LoadAssetAsync<StageDataObject>(DataAddress).WaitForCompletion();
            return dataObject.stageData;
        }
    }
}
