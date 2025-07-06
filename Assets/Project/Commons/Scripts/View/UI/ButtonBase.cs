using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public class ButtonBase : MonoBehaviour
    {
        const float MoveTime = .5f;
        const float ScaleRatio = 1.1f;
        
        Transform _transform;
        Vector3 _initialScale;
        
        readonly Subject<Unit> onPressed = new();
        public IObservable<Unit> OnPressed => onPressed;
        
        public bool IsActive { get; private set; }


        void Awake()
        {
            _transform = transform;
            _initialScale = _transform.localScale;
        }

        public void SetActive(bool active)
        {
            _transform.DOScale(_initialScale * (active? ScaleRatio : 1), MoveTime).SetEase(Ease.InOutQuart);
            IsActive = active;
        }
        
        public void Press()
        {
            if (!IsActive) return;
            
            onPressed.OnNext(Unit.Default);
        }
    }
}