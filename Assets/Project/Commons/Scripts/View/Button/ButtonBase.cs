using System;
using UniRx;
using UnityEngine;

namespace Project.Commons.Scripts.View
{
    public interface IButton
    {
        void Press();
    }
    
    public class ButtonBase : MonoBehaviour, IButton
    {
        bool _isPressed;
        
        readonly Subject<Unit> onPressed = new();
        public IObservable<Unit> OnPressed => onPressed;

        
        public void Press()
        {
            _isPressed = !_isPressed;
            if (!_isPressed) return;
            
            onPressed.OnNext(Unit.Default);
        }
    }
}