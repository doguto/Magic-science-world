using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Text;
using Project.Scripts.Extensions;

namespace Project.Scripts.Repository.AssetRepository
{
    public class StillAssetRepository : AssetRepositoryBase
    {
        public Sprite Load(string charaName, bool isCrazy)
        {
            string address = ZString.Format(
                "{0}/Character/{1}/Still/{1}{2}_Still.png",
                GamePath.TexturesPath,
                charaName,
                isCrazy ? "_Crazy" : ""
            );

            Sprite asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}
