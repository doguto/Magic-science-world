using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Text;

namespace Project.Scripts.Repository
{
    public class FaceAssetRepository : AssetRepository
    {
	
        public Sprite Load(string charaName, [CanBeNull] string variation = null, bool isCrazy = false)
        {
            string address = ZString.Format(
                "{0}/Character/{1}/Face/{1}{2}{3}_Face.png",
                TexturePath, 
                charaName,
                isCrazy ? "_Crazy" : "",
                string.IsNullOrEmpty(variation) ? "" : ZString.Concat("_", variation));
            
            
            var asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}