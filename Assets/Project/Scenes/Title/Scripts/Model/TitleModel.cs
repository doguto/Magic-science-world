using Project.Scripts.Model;
using Project.Scripts.Repository.AssetRepository;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.Model
{
    public class TitleModel : ModelBase
    {
        readonly AllCharacterStillAssetRepository allCharacterStillAssetRepository = new();

        readonly int clearedStageAmount;
        Sprite sprite;

        public Sprite MemberStillSprite { get; private set; }

        public TitleModel(int clearedStageAmount)
        {
            this.clearedStageAmount = clearedStageAmount;
        }

        public Sprite GetMemberStillSprite()
        {
            var characterCount = clearedStageAmount + 1;
            sprite ??= allCharacterStillAssetRepository.Load(characterCount);
            return sprite;
        }
    }
}
