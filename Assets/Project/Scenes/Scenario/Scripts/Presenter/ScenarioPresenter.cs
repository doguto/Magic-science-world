using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Scenes.Scenario.Scripts.Model;

namespace Project.Scenes.Scenario.Scripts.Presenter
{
    public class ScenarioPresenter : MonoBehaviour
    {
        [SerializeField] private ScenarioView view;

        private ScenarioModel _model;

        void Start()
        {
            // 現在のシーン名を取得（例: "Stage01"）
            // string sceneName = SceneManager.GetActiveScene().name.ToLower(); // e.g. "stage01"
            [SerializeField]
            [Tooltip("シーン名、後に取得ロジックを書く")]
            string sceneNumber;
            string resourcePath = $"Project/Scenes/Scenario/Data/conversation-{sceneNumber}";

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
        
        void Update()
        {
            if (_model == null) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_model.HasNext)
                    ShowNextLine();
                else
                    SceneManager.LoadScene("BattleWay"); // 遷移先シーン名は適宜変更！
            }
        }

        void ShowNextLine()
        {
            ScenarioLine line = _model.Next();
            view.Display(line.character, line.content);
        }
    }
}
