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

        void Start()
        {
            _titleBackgroundModel = new TitleBackgroundModel();
            
            titleMenuView.OnPressedStart.Subscribe(StartGame);
            
            SetTitleBackGround();
        }

        void StartGame(Unit _)
        {
            
        }

        void SetTitleBackGround()
        {
            var clearedStageAmount = _titleBackgroundModel.ClearedStageAmount;
            titleMenuView.SetBackGround(clearedStageAmount);
        }
    }
}