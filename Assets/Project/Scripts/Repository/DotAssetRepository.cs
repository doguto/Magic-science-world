using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Text;

namespace Project.Scripts.Repository
{
    public class DotAssetRepository : AssetRepository
    {
	
        public Sprite Load(string charaName, [CanBeNull] string variation = null )
        {
            string address = ZString.Format(
                "{0}/Character/{1}/Dot/{1}{2}_Dot.png",
                TexturePath, 
                charaName,
                string.IsNullOrEmpty(variation) ? "" :  ZString.Concat("_", variation));
            
            
            var asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}