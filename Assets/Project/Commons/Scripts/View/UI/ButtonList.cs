using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public class ButtonList : MonoBehaviour
    {
        [SerializeField] List<ButtonBase> buttons;
        
        public int ButtonIndex { get; private set; }
        public bool IsActive { get; private set; }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) PressButton();
        }

        public void Init(int index)
        {
            SetButtonIndex(index);
            buttons[ButtonIndex].SetActive(true);
        }

        public void SetActive(bool active)
        {
            foreach (var button in buttons)
            {
                button.SetActive(active);
            }
            IsActive = active;
        }
        
        public void SetActiveButton(int index)
        {
            SetButtonIndex(index);
            buttons[ButtonIndex].SetActive(false);
        }

        public void MoveToNextButton(bool isUp = true)
        {
            buttons[ButtonIndex].SetActive(false);
            SetButtonIndex( isUp ? ButtonIndex + 1 : ButtonIndex - 1 );
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
            ButtonIndex = index % buttons.Count;
        }
    }
}