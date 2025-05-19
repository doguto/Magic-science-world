using System;
using UniRx;

namespace Project.Scenes.Title.Scripts.ViewPort
{
    public interface ITitleView
    {
        public IObservable<Unit> OnStart { get; }
    }
}