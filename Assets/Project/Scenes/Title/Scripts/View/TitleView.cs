using System;
using Project.Scenes.Title.Scripts.ViewPort;
using Project.Commons.Scripts.View;
using UniRx;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.View
{
    public class TitleView : MonoBehaviour, ITitleView
    {
        [SerializeField] ButtonBase _button;

        public IObservable<Unit> OnStart => _button.OnPressed;

    }
}