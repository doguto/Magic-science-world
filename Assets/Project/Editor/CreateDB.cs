
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
    private string _sObjectName = "";
    private string _repositoryName = "";
    private string _menuName = "";
    
    
    //int,string,List<T>の数と変数名,型を管理
    private int _intFieldCount = 1;
    private List<string> _intFieldNames = new(){"id"};
    
    private int _stringFieldCount = 1;
    private List<string> _stringFieldNames = new(){"address"};
    
    private int _listFieldCount = 1;
    private List<string> _listFieldNames = new(){"data"};
    private List<string> _listFieldTypes = new(){"string"};
    
    private Vector2 _scrollPosition;

    private bool _shouldCreateAsset = false;
    
    
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
        _sObjectName = EditorGUILayout.TextField("オブジェクト名 ex:StageDataObject", _sObjectName);
        _repositoryName = EditorGUILayout.TextField("リポジトリ名 ex:StageModelRepository", _repositoryName);
        _menuName = EditorGUILayout.TextField("メニュー名 ex:Database/StageData", _menuName);
        EditorGUIUtility.labelWidth = 0; 
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Field Definitions");
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        
        //int型フィールド定義
        _intFieldCount = EditorGUILayout.IntField("Number of int fields", _intFieldCount);
        _intFieldCount = Mathf.Max(0, _intFieldCount);
        ResizeList(_intFieldNames, _intFieldCount);
        for (int i = 0; i < _intFieldCount; i++)
        {
            _intFieldNames[i] = EditorGUILayout.TextField($"int name {i+1}",_intFieldNames[i]);
        }
        EditorGUILayout.Space(5);
        
        //string型フィールド定義
        _stringFieldCount = EditorGUILayout.IntField("Number of string fields", _stringFieldCount);
        _stringFieldCount = Mathf.Max(0, _stringFieldCount);
        ResizeList(_stringFieldNames, _stringFieldCount);
        for (int i = 0; i < _stringFieldCount; i++)
        {
            _stringFieldNames[i] = EditorGUILayout.TextField($"string name {i+1}", _stringFieldNames[i]);
        }
        EditorGUILayout.Space(5);
        
        //list型フィールドと内部型定義
        _listFieldCount = EditorGUILayout.IntField("Number of list fields", _listFieldCount);
        _listFieldCount = Mathf.Max(0, _listFieldCount);
        ResizeList(_listFieldNames, _listFieldCount);
        ResizeList(_listFieldTypes, _listFieldCount);
        for (int i = 0; i < _listFieldCount; i++)
        {
            _listFieldNames[i] = EditorGUILayout.TextField($"list name {i+1}", _listFieldNames[i]);
            _listFieldTypes[i] = EditorGUILayout.TextField($"list type {i+1}", _listFieldTypes[i]);
        }
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space(20);
        
        //.assetファイル作るかどうかのチェックボックス
        _shouldCreateAsset = EditorGUILayout.Toggle("Create .asset file", _shouldCreateAsset);
        
        //生成開始ボタン
        if (GUILayout.Button("Generate"))
        {
            if (string.IsNullOrEmpty(_sObjectName))
            {
                EditorUtility.DisplayDialog("Error", "empty filename", "Ok");
                return;
            }

            GenerateFiles();
        }
        
    }
    
    
    

    
    //{_sObjectName}.csと{_repositoryName}.csを生成
    private void GenerateFiles()
    {
        string soPath = "Assets/Project/Scripts/Infra";
        if (!Directory.Exists(soPath))
        {
            Directory.CreateDirectory(soPath);
        }

        string soCode = GenerateScriptableObjectCode();
        File.WriteAllText($"{soPath}/{_sObjectName}.cs", soCode);
        
        
        string repoPath = "Assets/Project/Scripts/Repository";
        if (!Directory.Exists(repoPath))
        {
            Directory.CreateDirectory(repoPath);
        }
        
        string repoCode = GenerateRepositoryCode();
        File.WriteAllText($"{repoPath}/{_repositoryName}.cs", repoCode);
        
        AssetDatabase.Refresh();

        if (_shouldCreateAsset)
        {
            EditorApplication.delayCall += () =>
            {

                string assetPath = EditorUtility.SaveFilePanelInProject(
                    "Save ScriptableObject Asset",
                    _sObjectName + ".asset",
                    "asset",
                    "assetファイルの名前"
                );

                if (string.IsNullOrEmpty(assetPath)) return;
                
                ScriptableObject instance = CreateInstance(_sObjectName);
                
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
        sb.AppendLine($"    [CreateAssetMenu(fileName = \"{_sObjectName}\", menuName = \"{_menuName}\")]");
        sb.AppendLine($"    public class {_sObjectName} : ScriptableObject");
        sb.AppendLine("    {");
        foreach (var fieldName in _intFieldNames) { if (!string.IsNullOrEmpty(fieldName)) sb.AppendLine($"        public int {fieldName};"); }
        sb.AppendLine();
        foreach (var fieldName in _stringFieldNames) { if (!string.IsNullOrEmpty(fieldName)) sb.AppendLine($"        public string {fieldName};"); }
        sb.AppendLine();
        for (int i = 0; i < _listFieldCount; i++) { if (!string.IsNullOrEmpty(_listFieldNames[i]) && !string.IsNullOrEmpty(_listFieldTypes[i])) sb.AppendLine($"        public List<{_listFieldTypes[i]}> {_listFieldNames[i]} = new();"); }
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
        sb.AppendLine($"    public class {_repositoryName} : ModelRepository");
        sb.AppendLine("    {");
        sb.AppendLine($"        public static {_repositoryName} Instance {{ get; }} = new();");
        sb.AppendLine($"        public {_repositoryName}()");
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
