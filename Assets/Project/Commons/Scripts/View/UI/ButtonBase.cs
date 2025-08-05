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


        protected void Awake()
        {
            _transform = transform;
            _initialScale = _transform.localScale;
        }

        public void SetActive(bool active)
        {
            var endPosition = _initialScale * (active? ScaleRatio : 1);
            _transform.DOScale(endPosition, MoveTime).SetEase(Ease.InOutQuart);
            IsActive = active;
        }
        
        public void Press()
        {
            if (!IsActive) return;
            
            onPressed.OnNext(Unit.Default);
        }

        public void Move(Vector3 moveDistance)
        {
            var currentPosition = _transform.localPosition;
            var endPosition = currentPosition + moveDistance;
            _transform.DOLocalMove(endPosition, MoveTime).SetEase(Ease.InOutQuart);
        }
    }
}