using Project.Scripts.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Presenter
{
    public class GameBootStrapper : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void EnsureGlobalSceneLoaded()
        {
            var globalLoaded = false;
            var globalSceneName = SceneType.Global.ToSceneName();

            // 現在ロード済みシーンをチェック
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == globalSceneName)
                {
                    globalLoaded = true;
                    break;
                }
            }

            if (globalLoaded) return;

            Debug.Log("[GlobalSceneBootstrapper] Loading GlobalScene...");
            SceneManager.LoadScene(globalSceneName, LoadSceneMode.Additive);
        }
    }
}
