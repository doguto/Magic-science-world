using UnityEngine;

namespace Project.Scenes.Scenario.Scripts.Model
{
    [CreateAssetMenu(
        fileName = "ScenarioData",
        menuName = "Scenario/ScenarioData",
        order = 1)]
    public class ScenarioDataSO : ScriptableObject
    {
        public ScenarioLine[] scenarioLines;
    }

    [System.Serializable]
    public class ScenarioLine
    {
        public string character;
        [TextArea(3, 10)]
        public string content;
        public int faceNum;
    }
}
