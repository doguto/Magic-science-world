using Project.Scripts.Model;
using Project.Scripts.Repository.AssetRepository;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.Model;

public class TitleModel : ModelBase
{
    readonly AllCharacterStillAssetRepository allCharacterStillAssetRepository = new();

    readonly int clearedStageAmount;
    Sprite memberStillSprite;

    public TitleModel(int clearedStageAmount)
    {
        this.clearedStageAmount = clearedStageAmount;
    }

    public Sprite GetMemberStillSprite()
    {
        var characterCount = clearedStageAmount + 1;
        memberStillSprite ??= allCharacterStillAssetRepository.Load(characterCount);
        return memberStillSprite;
    }
}
