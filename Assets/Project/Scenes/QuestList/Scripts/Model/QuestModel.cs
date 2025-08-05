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
            Debug.Log("GetCharaImage");
            var sprite = await Addressables.LoadAssetsAsync<Sprite>("Assets/Project/Textures/CharaImage/Tatsumi.png").Task;
            
            return sprite.First();
        }
    }
}