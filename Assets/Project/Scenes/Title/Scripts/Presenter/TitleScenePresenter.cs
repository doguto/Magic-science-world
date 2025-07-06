using Project.Scenes.Title.Scripts.Model;
using Project.Scenes.Title.Scripts.View;
using UniRx;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.Presenter
{
    public class TitleScenePresenter : MonoBehaviour
    {
        [SerializeField] TitleMenuView titleMenuView;
        [SerializeField] TitleSettingModalView titleSettingModalView;
        
        TitleBackgroundModel _titleBackgroundModel;
        TitleGameStartModel _titleGameStartModel;

        void Start()
        {
            _titleBackgroundModel = new TitleBackgroundModel();
            _titleGameStartModel = new TitleGameStartModel();
            
            titleMenuView.OnPressedStart.Subscribe(StartGame);
            
            titleMenuView.Init();
            
            SetTitleBackGround();
        }

        void StartGame(Unit _)
        {
            _titleGameStartModel.StartGame();
        }

        void SetTitleBackGround()
        {
            var clearedStageAmount = _titleBackgroundModel.ClearedStageAmount;
            titleMenuView.SetBackGround(clearedStageAmount);
        }
    }
}