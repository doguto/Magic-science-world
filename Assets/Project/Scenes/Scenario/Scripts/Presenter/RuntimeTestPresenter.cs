using UnityEngine;
using Project.Scenes.Scenario.Scripts.Model.RuntimeDataTest;
// using Project.Scenes.Scenario.Scripts.Model.AssetMenuSample;

namespace Project.Scenes.Scenario.Scripts.Presenter
{
    public class RuntimeTestPresenter : MonoBehaviour
    {
        RuntimeData data;

        void Start()
        {
            // ランタイム生成
            data = ScriptableObject.CreateInstance<RuntimeData>();
            data.message = "Hello, Runtime!";
            data.counter = 0;
        }

        void Update()
        {
            data.counter++;
            Debug.Log($"{data.message} ({data.counter})");
        }
    }
}
