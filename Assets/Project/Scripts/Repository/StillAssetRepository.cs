using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Text;


namespace Project.Scripts.Repository
{
    public class StillAssetRepository : AssetRepository
    {
        public Sprite Load(string charaName, bool isCrazy)
        {
            string address = ZString.Format(
                "{0}/Character/{1}/Still/{1}{2}_Still.png",
                TexturePath,
                charaName,
                isCrazy ? "_Crazy" : ""
            );

            Sprite asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}