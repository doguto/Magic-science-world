using System;
using Project.Commons.Scripts.View.UI;
using UniRx;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.View
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField] ButtonBase _button;

        public IObservable<Unit> OnStart => _button.OnPressed;

    }
}