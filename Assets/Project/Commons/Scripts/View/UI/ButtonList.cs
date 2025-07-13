using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public class ButtonList : ButtonListBase
    {
        void Update()
        {
            if (!IsActive) return;

            if (MoveNextFlag) MoveNext();
            if (MoveBackFlag) MoveNext(false);
            if (Input.GetKeyDown(KeyCode.Space)) PressButton();
        }

        public override void Init(ButtonListType buttonListType, int index = 0, bool isActive = false)
        {
            this.buttonListType = buttonListType;
            
            SetButtonIndex(index);
            buttons[ButtonIndex].SetActive(true);
            
            SetActive(isActive);
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
    }
}