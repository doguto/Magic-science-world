using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public class ButtonList : MonoBehaviour
    {
        [SerializeField] List<ButtonBase> buttons;
        
        ButtonListType _buttonListType;
        
        public int ButtonIndex { get; private set; }
        public bool IsActive { get; private set; }

        bool MoveNextFlag => _buttonListType switch
        {
            ButtonListType.Vertical => Input.GetKeyDown(KeyCode.UpArrow),
            ButtonListType.Horizontal => Input.GetKeyDown(KeyCode.RightArrow),
            _ => false
        };
        
        bool MoveBackFlag => _buttonListType switch
        {
            ButtonListType.Vertical => Input.GetKeyDown(KeyCode.DownArrow),
            ButtonListType.Horizontal => Input.GetKeyDown(KeyCode.LeftArrow),
            _ => false
        };

        void Update()
        {
            if (!IsActive) return;

            if (MoveNextFlag) MoveNext();
            if (MoveBackFlag) MoveNext(false);
            if (Input.GetKeyDown(KeyCode.Space)) PressButton();
        }

        public void Init(ButtonListType buttonListType, int index = 0, bool isActive = false)
        {
            _buttonListType = buttonListType;
            
            SetButtonIndex(index);
            buttons[ButtonIndex].SetActive(true);
            
            SetActive(isActive);
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }
        
        public void SetActiveButton(int index, bool isActive = false)
        {
            SetButtonIndex(index);
            buttons[ButtonIndex].SetActive(isActive);
        }

        public void MoveNext(bool isUp = true)
        {
            buttons[ButtonIndex].SetActive(false);
            SetButtonIndex( isUp ? ButtonIndex - 1 : ButtonIndex + 1 );
            buttons[ButtonIndex].SetActive(true);
        }

        public IObservable<Unit> GetButtonEvent(int index)
        {
            return buttons[index].OnPressed;
        }

        public void PressButton() => PressButton(ButtonIndex);
        public void PressButton(int index)
        {
            if (!IsActive) return;
            
            buttons[index].Press();
        }
        
        void SetButtonIndex(int index)
        {
            ButtonIndex = (index + buttons.Count) % buttons.Count;
        }
    }

    public enum ButtonListType
    {
        Vertical,
        Horizontal,
    }
}