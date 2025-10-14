
using UnityEditor;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// DB作成の簡易化を目的に作られたスクリプト
/// UnityEditorのウィンドウメニュー→Tools→Create DB Generatorで使用
/// </summary>

public class CreateDB : EditorWindow 
{
    private string sObjectName = "";
    private string repositoryName = "";
    private string menuName = "";
    
    
    //int,string,List<T>の数と変数名,型を管理
    private int intFieldCount = 1;
    private List<string> intFieldNames = new(){"id"};
    
    private int stringFieldCount = 1;
    private List<string> stringFieldNames = new(){"address"};
    
    private int listFieldCount = 1;
    private List<string> listFieldNames = new(){"data"};
    private List<string> listFieldTypes = new(){"string"};
    
    private Vector2 scrollPosition;

    private bool shouldCreateAsset = false;
    
    
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
        sObjectName = EditorGUILayout.TextField("オブジェクト名 ex:StageDataObject", sObjectName);
        repositoryName = EditorGUILayout.TextField("リポジトリ名 ex:StageModelRepository", repositoryName);
        menuName = EditorGUILayout.TextField("メニュー名 ex:Database/StageData", menuName);
        EditorGUIUtility.labelWidth = 0; 
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Field Definitions");
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        //int型フィールド定義
        intFieldCount = EditorGUILayout.IntField("Number of int fields", intFieldCount);
        intFieldCount = Mathf.Max(0, intFieldCount);
        ResizeList(intFieldNames, intFieldCount);
        for (int i = 0; i < intFieldCount; i++)
        {
            intFieldNames[i] = EditorGUILayout.TextField($"int name {i+1}",intFieldNames[i]);
        }
        EditorGUILayout.Space(5);
        
        //string型フィールド定義
        stringFieldCount = EditorGUILayout.IntField("Number of string fields", stringFieldCount);
        stringFieldCount = Mathf.Max(0, stringFieldCount);
        ResizeList(stringFieldNames, stringFieldCount);
        for (int i = 0; i < stringFieldCount; i++)
        {
            stringFieldNames[i] = EditorGUILayout.TextField($"string name {i+1}", stringFieldNames[i]);
        }
        EditorGUILayout.Space(5);
        
        //list型フィールドと内部型定義
        listFieldCount = EditorGUILayout.IntField("Number of list fields", listFieldCount);
        listFieldCount = Mathf.Max(0, listFieldCount);
        ResizeList(listFieldNames, listFieldCount);
        ResizeList(listFieldTypes, listFieldCount);
        for (int i = 0; i < listFieldCount; i++)
        {
            listFieldNames[i] = EditorGUILayout.TextField($"list name {i+1}", listFieldNames[i]);
            listFieldTypes[i] = EditorGUILayout.TextField($"list type {i+1}", listFieldTypes[i]);
        }
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space(20);
        
        //.assetファイル作るかどうかのチェックボックス
        shouldCreateAsset = EditorGUILayout.Toggle("Create .asset file", shouldCreateAsset);
        
        //生成開始ボタン
        if (GUILayout.Button("Generate"))
        {
            if (string.IsNullOrEmpty(sObjectName))
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
        string soPath = "Assets/Project/Scripts/Infra";
        if (!Directory.Exists(soPath))
        {
            Directory.CreateDirectory(soPath);
        }

        string soCode = GenerateScriptableObjectCode();
        File.WriteAllText($"{soPath}/{sObjectName}.cs", soCode);
        
        
        string repoPath = "Assets/Project/Scripts/Repository";
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
        foreach (var fieldName in intFieldNames) { if (!string.IsNullOrEmpty(fieldName)) sb.AppendLine($"        public int {fieldName};"); }
        sb.AppendLine();
        foreach (var fieldName in stringFieldNames) { if (!string.IsNullOrEmpty(fieldName)) sb.AppendLine($"        public string {fieldName};"); }
        sb.AppendLine();
        for (int i = 0; i < listFieldCount; i++) { if (!string.IsNullOrEmpty(listFieldNames[i]) && !string.IsNullOrEmpty(listFieldTypes[i])) sb.AppendLine($"        public List<{listFieldTypes[i]}> {listFieldNames[i]} = new();"); }
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
    
    
    private void ResizeList(List<string> list, int size)
    {
        while (list.Count < size)
        {
            list.Add("");
        }
        while (list.Count > size)
        {
            list.RemoveAt(list.Count - 1);
        }
    }
}
