using Cysharp.Threading.Tasks;
using Project.Scripts.Extensions;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Model
{
    public class ModelBase
    {
        public T LoadAsset<T>(string assetAddress)
        {
            var address = $"{GamePath.TexturesPath}/{assetAddress}";
            return Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetAddress)
        {
            var address = $"{GamePath.TexturesPath}/{assetAddress}";
            var task = Addressables.LoadAssetAsync<T>(address);
            return await task.Task;
        }

        public T LoadData<T>(string dataName)
        {
            var address = $"{GamePath.DataStorepath}/{dataName}.asset";
            return Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
        }

        public async UniTask<T> LoadDataAsync<T>(string dataName)
        {
            var address = $"{GamePath.DataStorepath}/{dataName}.asset";
            var task = Addressables.LoadAssetAsync<T>(address);
            return await task.Task;
        }
    }
}
