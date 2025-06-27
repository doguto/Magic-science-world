using UnityEditor;
using UnityEngine;
using System.IO;
using Project.Scenes.Scenario.Scripts.Model;

namespace Project.Scenes.Scenario.Scripts.Model
{
public static class ScenarioJsonImporter
{
    [MenuItem("Tools/Import Scenario from JSON")]
    public static void ImportAll()
    {
        string jsonDir = "Assets/Editor/Novel/";
        string outputDir = "Assets/Resources/Novel/";

        foreach (string path in Directory.GetFiles(jsonDir, "*.json"))
        {
            string json = File.ReadAllText(path);

            ScenarioLineWrapper wrapper = new ScenarioLineWrapper();
            wrapper.lines = JsonUtility.FromJson<ScenarioLineArray>(WrapArray(json)).lines;

            ScenarioDataSO asset = ScriptableObject.CreateInstance<ScenarioDataSO>();
            asset.scenarioLines = wrapper.lines;

            string fileName = Path.GetFileNameWithoutExtension(path);
            AssetDatabase.CreateAsset(asset, $"{outputDir}/{fileName}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("全シナリオをインポートしました！");
    }

    // JsonUtilityは配列を直接パースできないので、ラッパーで包む
    private static string WrapArray(string jsonArray)
    {
        return "{\"lines\":" + jsonArray + "}";
    }

    [System.Serializable]
    private class ScenarioLineArray
    {
        public ScenarioLine[] lines;
    }

    private class ScenarioLineWrapper
    {
        public ScenarioLine[] lines;
    }
    }
}
