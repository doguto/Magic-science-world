using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Project.Commons.Scripts.Model
{
    public class ModelBase
    {
        const string AssetAddressRoot = "Assets/Project/Textures";
        const string DataAddressRoot = "Assets/Project/Commons/DataBase/ScriptableObjects";

        public static T LoadAsset<T>(string assetAddress)
        {
            var address = $"{AssetAddressRoot}/{assetAddress}";
            return Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
        }

        public static UniTask<T> LoadAssetAsync<T>(string assetAddress)
        {
            var address = $"{DataAddressRoot}/{assetAddress}";
            var task = Addressables.LoadAssetAsync<T>(address);
            return task.Task.AsUniTask();
        }

        public static T LoadData<T>(string dataName)
        {
            var address = $"{DataAddressRoot}/{dataName}.asset";
            return Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
        }

        public static UniTask<T> LoadDataAsync<T>(string dataName)
        {
            var address = $"{DataAddressRoot}/{dataName}.asset";
            var task = Addressables.LoadAssetAsync<T>(address);
            return task.Task.AsUniTask();
        }
    }
}
