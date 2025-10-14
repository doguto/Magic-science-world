
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// DB作成の簡易化を目的に作られたスクリプト
/// UnityEditorのウィンドウメニュー→Tools→Create DB Generatorで使用
/// </summary>

public class CreateDB : EditorWindow
{
    private string soDataName = "";
    private string sObjectName = "";
    private string repositoryName = "";
    private string menuName = "";
    
    
    //int,string,List<T>の数と変数名,型を管理
    [System.Serializable]
    private class ColumnField
    {
        public string name = "";
        public string type = "";

        public bool IsNullOrEmpty
        {
            get => string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type);
        }
    }
    
    private List<ColumnField> columnFields = new() {new ColumnField()};
    
    private Vector2 scrollPosition;

    private bool shouldCreateAsset = true;
    
    
    //メニュー生成
    [MenuItem("Tools/Create DB Generator")]
    private static void ShowWindow()
    {
        GetWindow<CreateDB>("DB Generator");
    }
    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("ScriptableObject Settings");
        EditorGUIUtility.labelWidth = 250; 
        soDataName = EditorGUILayout.TextField("データ名", soDataName);
        sObjectName = $"{soDataName}DataObject";
        repositoryName = $"{soDataName}ModelRepository";
        menuName = $"Database/{soDataName}Data";
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("オブジェクト名", sObjectName);
        EditorGUILayout.TextField("リポジトリ名", repositoryName);
        EditorGUILayout.TextField("メニューパス", menuName);
        EditorGUI.EndDisabledGroup();
        EditorGUIUtility.labelWidth = 0; 
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Field Definitions");
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("型", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("変数名");
        EditorGUILayout.EndHorizontal();
        
        //フィールド定義
        for (int i = 0; i < columnFields.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            columnFields[i].type = EditorGUILayout.TextField(columnFields[i].type);
            columnFields[i].name = EditorGUILayout.TextField(columnFields[i].name);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space(10);
        
        //追加・削除ボタン
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("変数を追加"))
        {
            columnFields.Add(new ColumnField());
        }
        
        EditorGUI.BeginDisabledGroup(columnFields.Count == 0);
        if (GUILayout.Button("変数を削除"))
        {
            columnFields.RemoveAt(columnFields.Count - 1);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        
        
        //.assetファイル作るかどうかのチェックボックス
        shouldCreateAsset = EditorGUILayout.Toggle("Create .asset file", shouldCreateAsset);
        EditorGUILayout.Space(20);
        
        //生成開始ボタン
        if (GUILayout.Button("Generate"))
        {
            if (string.IsNullOrEmpty(soDataName))
            {
                EditorUtility.DisplayDialog("Error", "empty filename", "Ok");
                return;
            }

            GenerateFiles();
        }
        
    }
    
    
    

    
    //{sObjectName}.csと{repositoryName}.csを生成
    private void GenerateFiles()
    {
        const string soPath = "Assets/Project/Scripts/Infra";
        if (!Directory.Exists(soPath))
        {
            Directory.CreateDirectory(soPath);
        }

        string soCode = GenerateScriptableObjectCode();
        File.WriteAllText($"{soPath}/{sObjectName}.cs", soCode);
        
        
        const string repoPath = "Assets/Project/Scripts/Repository";
        if (!Directory.Exists(repoPath))
        {
            Directory.CreateDirectory(repoPath);
        }
        
        string repoCode = GenerateRepositoryCode();
        File.WriteAllText($"{repoPath}/{repositoryName}.cs", repoCode);
        
        AssetDatabase.Refresh();

        if (shouldCreateAsset)
        {
            EditorApplication.delayCall += () =>
            {

                string assetPath = EditorUtility.SaveFilePanelInProject(
                    "Save ScriptableObject Asset",
                    sObjectName + ".asset",
                    "asset",
                    "assetファイルの名前"
                );

                if (string.IsNullOrEmpty(assetPath)) return;
                
                ScriptableObject instance = CreateInstance(sObjectName);
                
                AssetDatabase.CreateAsset(instance, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = instance;
                
                EditorUtility.DisplayDialog("Success", "Generated Scripts and asset successfully", "Ok");
                this.Close();
            };
        }
        else
        {
            EditorUtility.DisplayDialog("Success", "Generated Scripts successfully", "Ok");
            this.Close();
        }
    }

    //ScriptableObjectの定義コード
    //Project.Scripts.Infra内が前提
    private string GenerateScriptableObjectCode()
    {
        StringBuilder sb = new();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine("namespace Project.Scripts.Infra");
        sb.AppendLine("{");
        sb.AppendLine($"    [CreateAssetMenu(fileName = \"{sObjectName}\", menuName = \"{menuName}\")]");
        sb.AppendLine($"    public class {sObjectName} : ScriptableObject");
        sb.AppendLine("    {");
        for (int i = 0; i < columnFields.Count; i++)
        {
            if (!columnFields[i].IsNullOrEmpty) sb.AppendLine($"        public {columnFields[i].type} {columnFields[i].name};");
        }
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("}");
        return sb.ToString();
    }

    //Repositoryのコード
    //Project.Scripts.Repository内が前提
    private string GenerateRepositoryCode()
    {
        StringBuilder sb = new();
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using Project.Scenes.QuestList.Scripts.Model;");
        sb.AppendLine("using Project.Scripts.Infra;");
        sb.AppendLine("using UnityEngine.AddressableAssets;");
        sb.AppendLine();
        sb.AppendLine("namespace Project.Scripts.Repository");
        sb.AppendLine("{");
        sb.AppendLine($"    public class {repositoryName} : ModelRepository");
        sb.AppendLine("    {");
        sb.AppendLine($"        public static {repositoryName} Instance {{ get; }} = new();");
        sb.AppendLine($"        public {repositoryName}()");
        sb.AppendLine("        {");
        sb.AppendLine();
        sb.AppendLine("        }");
        sb.AppendLine("//必要に応じてGetとLoadData");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return  sb.ToString();
    }
    
    
}
