using UnityEngine.UI;
using UnityEngine.AddressableAssets;


namespace Project.Scripts.Repository
{
    public class StillAssetRepository : AssetRepository
    {
	
        public Image Load(string charaName, string faceName)
        {
            string assetPath = $"{TexturePath}/Character/{charaName}/Still";  
            string assetName = $"{charaName}_{faceName}_Still.png";
            string address = $"{assetPath}/{assetName}";
            
            var asset = Addressables.LoadAssetAsync<Image>(address).WaitForCompletion();
            return asset;
        }
    }
}