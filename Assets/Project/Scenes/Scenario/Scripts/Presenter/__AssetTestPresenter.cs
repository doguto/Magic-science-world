using UnityEngine;
using Project.Scenes.Scenario.Scripts.Model;

namespace Project.Scenes.Scenario.Scripts.Presenter
{
    public class AssetTestPresenter : MonoBehaviour
    {
        [SerializeField] ScenarioDataSO scenarioData;

        void Start()
        {
            Debug.Log($"Scenario: {scenarioData.scenarioLines[0].character}, {scenarioData.scenarioLines[0].content}, {scenarioData.scenarioLines[0].faceNum}");
        }
        void Update()
        {
            Debug.Log($"Scenario: {scenarioData.scenarioLines[0].character}, {scenarioData.scenarioLines[0].content}, {scenarioData.scenarioLines[0].faceNum}");
        }
    }
}
