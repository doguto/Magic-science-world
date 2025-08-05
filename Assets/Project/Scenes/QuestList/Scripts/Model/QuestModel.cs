using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scenes.QuestList.Scripts.Model
{
    public class QuestModel
    {
        public async UniTask<Sprite> GetCharaImage(int stageNumber)
        {
            var sprite = await Addressables.LoadAssetAsync<Sprite>("Assets/Project/Textures/CharaImage/Tatsumi.png").Task;
            return sprite;
        }
    }
}