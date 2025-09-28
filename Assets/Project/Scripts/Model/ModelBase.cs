using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Model
{
    public class ModelBase
    {
        const string AssetAddressRoot = "Assets/Project/Textures";
        const string DataAddressRoot = "Assets/Project/DataStore";

        public T LoadAsset<T>(string assetAddress)
        {
            var address = $"{AssetAddressRoot}/{assetAddress}";
            return Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetAddress)
        {
            var address = $"{AssetAddressRoot}/{assetAddress}";
            var task = Addressables.LoadAssetAsync<T>(address);
            return await task.Task;
        }

        public T LoadData<T>(string dataName)
        {
            var address = $"{DataAddressRoot}/{dataName}.asset";
            return Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
        }

        public async UniTask<T> LoadDataAsync<T>(string dataName)
        {
            var address = $"{DataAddressRoot}/{dataName}.asset";
            var task = Addressables.LoadAssetAsync<T>(address);
            return await task.Task;
        }
    }
}
