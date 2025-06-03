using System;
using Project.Commons.Scripts.View.UI;
using UniRx;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.View
{
    public class TitleMenuView : MonoBehaviour
    {
        [SerializeField] ButtonList buttonList;  // 0: Start, 1: Option

        public IObservable<Unit> OnPressedStart => buttonList.GetButtonEvent(0);
        
        void Start()
        {
            buttonList.Init(0);
        }

        void Update()
        {
            SwitchSelectButton();
        }

        void SwitchSelectButton()
        {
            if (!buttonList.IsActive) return;
            
            if (Input.GetKeyDown(KeyCode.UpArrow)) buttonList.MoveToNextButton();
            if (Input.GetKeyDown(KeyCode.DownArrow)) buttonList.MoveToNextButton(false);
        }
    }
}