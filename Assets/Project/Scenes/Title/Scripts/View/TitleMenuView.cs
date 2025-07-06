using System;
using System.Collections.Generic;
using NUnit.Framework;
using Project.Commons.Scripts.View.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scenes.Title.Scripts.View
{
    public class TitleMenuView : MonoBehaviour
    {
        [SerializeField] ButtonList buttonList;  // 0: Start, 1: Option
        [SerializeField] List<Sprite> backgroundSprites;
        [SerializeField] SpriteRenderer backgroundRenderer;

        public IObservable<Unit> OnPressedStart => buttonList.GetButtonEvent(0);
        public IObservable<Unit> OnPressedExit => buttonList.GetButtonEvent(1);

        public void Init()
        {
            buttonList.Init(ButtonListType.Vertical, 0, true);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetBackGround(int clearedStageAmount)
        {
            backgroundRenderer.sprite = backgroundSprites[clearedStageAmount];
        }
    }
}