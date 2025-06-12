using System;
using UniRx;
using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public class ButtonBase : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        bool _isPressed;
        
        readonly Subject<Unit> onPressed = new();
        public IObservable<Unit> OnPressed => onPressed;

        public void SetActive(bool active)
        {
            IsActive = active;
        }
        
        public void Press()
        {
            if (!IsActive) return;
            
            _isPressed = !_isPressed;
            if (!_isPressed) return;
            
            onPressed.OnNext(Unit.Default);
        }
    }
}