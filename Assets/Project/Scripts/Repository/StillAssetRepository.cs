using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Text;


namespace Project.Scripts.Repository
{
    public class StillAssetRepository : AssetRepository
    {
	
        public Sprite Load(string charaName, bool isCrazy)
        {
            StringBuilder address = new StringBuilder(TexturePath);
            address.Append($"/Character/{charaName}/Still");
            
            Sprite asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}