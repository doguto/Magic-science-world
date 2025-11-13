using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Scripts.Model;
using Project.Scenes.Title.Scripts.Model;
using Project.Scenes.Title.Scripts.View;
using Project.Scripts.Presenter;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scenes.Title.Scripts.Presenter
{
    public class TitleScenePresenter : MonoPresenter
    {
        [SerializeField] TitleMenuView titleMenuView;
        [SerializeField] TitleSettingModalView titleSettingModalView;
        
        TitleBackgroundModel titleBackgroundModel;
        TitleGameStartModel titleGameStartModel;

        void Start()
        {
            titleBackgroundModel = new TitleBackgroundModel();
            titleGameStartModel = new TitleGameStartModel();

            titleMenuView.OnPressedStart.Subscribe(x => StartGame(x).Forget());
            titleMenuView.OnPressedExit.Subscribe(ExitGame);

            titleMenuView.Init();

            SetTitleBackGround();
        }

        async UniTask StartGame(Unit _)
        {
            titleGameStartModel.StartGame();
            await SceneManager.LoadSceneAsync(SceneRouterModel.StageList, LoadSceneMode.Additive).ToUniTask();

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneRouterModel.StageList));
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