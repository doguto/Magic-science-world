using System;
using System.Collections.Generic;
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

        public StageModel GetById(int stageId)
        {
            var model = stageModels.Find(m => m.StageData.id == stageId);
            if (model != null) return model;

            var data = stageData.Find(m => m.id == stageId);
            if (data == null) throw new Exception($"StageId {stageId} のデータが存在しません.");

            var newModel = new StageModel(data);
            stageModels.Add(newModel);
            return newModel;
        }

        public List<StageModel> GetAll() => stageModels;

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
