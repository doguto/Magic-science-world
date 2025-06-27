using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Scenes.Scenario.Scripts.Model;
using Project.Scenes.Scenario.Scripts.View;

namespace Project.Scenes.Scenario.Scripts.Presenter
{
    public class ScenarioPresenter : MonoBehaviour
    {
        [SerializeField] private ScenarioView view;
        [Range(1, 15)]
        [SerializeField] private int sceneNumber = 1;
        private ScenarioModel _model;
        void MyFunc()
        {
            
            // 現在のシーン名を取得（例: "Stage01"）
            // string sceneName = SceneManager.GetActiveScene().name.ToLower(); // e.g. "stage01"
            string resourcePath = $"Novel/conversation-{sceneNumber}";

            // シーン名を取得
            string sceneName = SceneManager.GetActiveScene().name;
        

            // Resources/Scenario/sceneName.asset を読み込む
            ScenarioDataSO scenarioData = Resources.Load<ScenarioDataSO>(resourcePath);


            if (scenarioData == null)
            {
                Debug.LogError($"シナリオデータが読み込めませんでした: {resourcePath}");
                return;
            }

            _model = new ScenarioModel(scenarioData.scenarioLines);
            ShowNextLine(); 
        }

        void Start()
        {
            MyFunc();
        }
        
        void Update()
        {
            if (_model == null) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_model.HasNext)
                    ShowNextLine();
                else
                {
                    sceneNumber++;
                    MyFunc();
                }
            }
        }

        void ShowNextLine()
        {
            ScenarioLine line = _model.Next();
            view.Display(line.character, line.content, line.faceNum);
        }
    }
}
