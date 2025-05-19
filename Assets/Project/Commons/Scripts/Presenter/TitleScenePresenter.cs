using System;
using Project.Commons.Scripts.PresenterPort;
using Project.Scenes.Title.Scripts.ViewPort;
using UniRx;

namespace Project.Commons.Scripts.Presenter
{
    public class TitleScenePresenter : ITitleScenePresenter
    {
        readonly ITitleView titleView;
        
        public IObservable<Unit> OnStart => titleView.OnStart;
        public void CloseScene()
        {
            
        }
    }
}