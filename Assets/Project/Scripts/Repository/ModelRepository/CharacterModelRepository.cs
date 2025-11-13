using System;
using System.Collections.Generic;
using Project.Scripts.Infra;
using Project.Scripts.Model;

namespace Project.Scripts.Repository.ModelRepository
{
    public class CharacterModelRepository : ModelRepositoryBase
    {
        public static CharacterModelRepository instance = new();
        
        readonly List<CharacterData> characterData = new();
        readonly List<CharacterModel> characterModels = new();

        public CharacterModelRepository()
        {
            dataName = "CharacterData";
            characterData = LoadData();
            foreach (var data in characterData)
            {
                characterModels.Add(new CharacterModel(data));
            }
        }

        public CharacterModel Get(int characterId)
        {
            var index = characterId - 1;
            if (index < 0 || characterModels.Count <= index) return null;
            if (characterModels.Count > index) return characterModels[index];

            for (var i = characterModels.Count; i < characterData.Count; i++)
            {
                characterModels.Add(new CharacterModel(characterData[i]));
            }

            return characterModels[index];
        }

        public CharacterModel GetByName(string name)
        {
            var model = characterModels.Find(m => m.Name == name);
            if (model == null) throw new Exception($"キャラクター名 {name} のキャラクターデータは存在しません.");

            return model;
        }

        public List<CharacterModel> All() => characterModels;

        public void Refresh()
        {
            characterData.Clear();
        }

        List<CharacterData> LoadData()
        {
            var dataObject = LoadDataObject<CharacterDataObject>();
            return dataObject.characterData;
        }
    }
}
