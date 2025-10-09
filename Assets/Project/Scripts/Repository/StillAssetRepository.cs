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
            address.Append("/Character/");
            address.Append(charaName);
            address.Append("/Still/");
            address.Append(charaName);
            address.Append(isCrazy ? "_Crazy" : "");
            address.Append("_Still.png");
            
            Debug.Log(address);
            
            Sprite asset = Addressables.LoadAssetAsync<Sprite>(address.ToString()).WaitForCompletion();
            return asset;
        }
    }
}