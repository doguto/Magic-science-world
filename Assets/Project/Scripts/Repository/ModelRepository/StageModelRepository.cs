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

        readonly UserDataModel userModel;

        public StageModelRepository()
        {
            dataName = "StageData";
            stageData = LoadData();

            userModel = UserModelRepository.Instance.Get();

            foreach (var data in stageData)
            {
                var stageNumber = data.stageNumber;
                stageModels.Add(new StageModel(data, userModel.IsOpenedStage(stageNumber), userModel.IsClearedStage(stageNumber)));
            }
        }

        public StageModel GetById(string stageId)
        {
            var model = stageModels.Find(m => m.StageData.id == stageId);
            if (model != null) return model;

            var data = stageData.Find(m => m.id == stageId);
            if (data == null) throw new Exception($"StageId {stageId} のデータが存在しません.");

            var stageNumber = data.stageNumber;
            var newModel = new StageModel(data, userModel.IsOpenedStage(stageNumber), userModel.IsClearedStage(stageNumber));
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
            var dataObject = LoadDataObject<StageDataObject>();
            return dataObject.stageData;
        }
    }
}
