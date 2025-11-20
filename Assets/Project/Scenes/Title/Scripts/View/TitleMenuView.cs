using System;
using System.Collections.Generic;
using Project.Commons.Button.Scripts.View;
using UniRx;
using UnityEngine;

namespace Project.Scenes.Title.Scripts.View;

public class TitleMenuView : MonoBehaviour
{
    [SerializeField] ButtonList buttonList;  // 0: Start, 1: Exit
    [SerializeField] List<Sprite> backgroundSprites;
    [SerializeField] SpriteRenderer memberStillRenderer;

    public IObservable<Unit> OnPressedStart => buttonList.GetButtonEvent(0);
    public IObservable<Unit> OnPressedExit => buttonList.GetButtonEvent(1);

    public void Init(Sprite memberStillSprite)
    {
        memberStillRenderer.sprite = memberStillSprite;
        buttonList.Init(ButtonListType.Vertical, 0, true);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetBackGround(int clearedStageAmount)
    {
        memberStillRenderer.sprite = backgroundSprites[clearedStageAmount];
    }
}