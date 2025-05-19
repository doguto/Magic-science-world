using System;
using UniRx;

namespace Project.Commons.Scripts.PresenterPort
{
    public interface ITitleScenePresenter
    {
        IObservable<Unit> OnStart { get; }

        void CloseScene();
    }
}