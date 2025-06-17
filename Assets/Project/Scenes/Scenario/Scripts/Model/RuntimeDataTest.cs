using UnityEngine;

namespace Project.Scenes.Scenario.Scripts.Model.RuntimeDataTest
{
    [System.Serializable]
    public class RuntimeData : ScriptableObject
    {
        public string message;
        public int counter;
    }
}