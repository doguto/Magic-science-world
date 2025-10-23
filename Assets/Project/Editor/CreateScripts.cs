using UnityEditor;
using UnityEngine;
using System.IO;

namespace Project.Editor
{
    public class CreateScripts : EditorWindow
    {
        private string scriptName = "";

        [MenuItem("Tools/CreateScripts")]
        public static void ShowWindow()
        {
            GetWindow<CreateScripts>("CreateScripts");
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Create Scripts", EditorStyles.boldLabel);
            EditorGUIUtility.labelWidth = 250;
            scriptName = EditorGUILayout.TextField(scriptName);
            EditorGUILayout.LabelField("");
            EditorGUILayout.Space(5);

            if (GUILayout.Button("Create Script"))
            {
                if (string.IsNullOrEmpty(scriptName))
                {
                    EditorUtility.DisplayDialog("Error", "Please enter a script name", "OK");
                    return;
                }

                CreateAssemblyReferences();
                this.Close();
            }
        }

        void CreateAssemblyReferences()
        {
            const string scriptsBasePath = "Assets/Project/Commons";
            string[] asmdefNames = { "Model", "Presenter", "View" };
            
            string path1 = Path.Combine(scriptsBasePath, scriptName);
            string path2 =  Path.Combine(path1, "Scripts");

            Directory.CreateDirectory(path2);
            Directory.CreateDirectory(Path.Combine(path1, "Prefabs"));
            foreach (string asmdefName in asmdefNames)
            {
                string subFolderPath = Path.Combine(path2, asmdefName);
                if (Directory.Exists(subFolderPath))
                {
                    Debug.LogWarning($"{asmdefName} already exists");
                    continue;
                }
                Directory.CreateDirectory(subFolderPath);
                
                string asmrefPath = Path.Combine(subFolderPath, asmdefName+".asmref");

                string asmrefContent = 
                    $@"{{
                        ""reference"": ""{asmdefName}""
                    }}";
                File.WriteAllText(asmrefPath, asmrefContent);
                Debug.Log($"Created asmref: {asmrefPath}");
            }
            
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Done", $"Assembly References for '{scriptName}' Created!" , "OK");

        }
    }
}