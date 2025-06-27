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

        void Start()
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
            ScenarioLine line = _model.GetCurrentLine();
            string mainCharacterKey = _model.GetMainCharacterKey();
            string enemyCharacterKey = _model.GetEnemyCharacterKey();
            view.InitStillImage(mainCharacterKey, enemyCharacterKey, line.faceType.ToString());
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
                    // TODO: ボス遷移or道中遷移orシーン終了
                    sceneNumber++;
                    Start();
                }
            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (_model.HasPrevious)
                    ShowPreviousLine();
                else
                {
                    sceneNumber--;
                    Start();
                }
            }
        }
#endif
        void ShowNextLine()
        {
            ScenarioLine line = _model.Next();
            string speakingCharacterKey = _model.GetSpeakingCharacterKey();
            view.Display(new ScenarioView.DisplayData
            {
                character = line.character,
                content = line.content,
                faceType = line.faceType.ToString(),
                characterKey = speakingCharacterKey
            });
        }

        void ShowPreviousLine()
        {
            ScenarioLine line = _model.UndoDev();
            string speakingCharacterKey = _model.GetSpeakingCharacterKey();
            view.Display(new ScenarioView.DisplayData
            {
                character = line.character,
                content = line.content,
                faceType = line.faceType.ToString(),
                characterKey = speakingCharacterKey
            });
        }
    }
}
