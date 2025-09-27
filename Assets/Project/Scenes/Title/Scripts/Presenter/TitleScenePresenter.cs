using Project.Scripts.Model;
using Project.Scenes.Title.Scripts.Model;
using Project.Scenes.Title.Scripts.View;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scenes.Title.Scripts.Presenter
{
    public class TitleScenePresenter : MonoBehaviour
    {
        [SerializeField] TitleMenuView titleMenuView;
        [SerializeField] TitleSettingModalView titleSettingModalView;
        
        TitleBackgroundModel titleBackgroundModel;
        TitleGameStartModel titleGameStartModel;

        void Start()
        {
            titleBackgroundModel = new TitleBackgroundModel();
            titleGameStartModel = new TitleGameStartModel();
            
            titleMenuView.OnPressedStart.Subscribe(StartGame);
            titleMenuView.OnPressedExit.Subscribe(ExitGame);
            
            titleMenuView.Init();
            
            SetTitleBackGround();
        }

        void StartGame(Unit _)
        {
            titleGameStartModel.StartGame();
            SceneManager.LoadScene(SceneRouterModel.QuestList, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneRouterModel.QuestList));
            SceneManager.UnloadSceneAsync(gameObject.scene.name);
        }

        void ExitGame(Unit _)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();//ゲームプレイ終了
#endif
        }

        void SetTitleBackGround()
        {
            var clearedStageAmount = titleBackgroundModel.ClearedStageAmount;
            titleMenuView.SetBackGround(clearedStageAmount);
        }
    }
}