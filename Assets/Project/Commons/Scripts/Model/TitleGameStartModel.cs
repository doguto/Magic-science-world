using UniRx;
using Project.Commons.Scripts.PresenterPort;
using UnityEngine;

namespace Project.Commons.Scripts.Model
{
    public class TitleGameStartModel
    {
        readonly ITitleScenePresenter titleScenePresenter;

        public TitleGameStartModel(ITitleScenePresenter titleScenePresenter)
        {
            this.titleScenePresenter = titleScenePresenter;
            
            Initialize();
        }

        void Initialize()
        {
            titleScenePresenter.OnStart.Subscribe(x => Execute());
        }

        void Execute()
        {
            Debug.Log("Modelに到達!!");
            titleScenePresenter.CloseScene();
        }
    }
}