using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public class ButtonList : MonoBehaviour
    {
        [SerializeField] List<ButtonBase> buttons;
        
        public int ButtonIndex { get; private set; }
        public bool IsActive { get; private set; }

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

        void SetButtonIndex(int index)
        {
            ButtonIndex = index % buttons.Count;
        }
    }
}