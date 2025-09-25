using System;
using DG.Tweening;
using Project.Commons.Scripts.View.UI;
using UnityEngine;
using UnityEngine.UI;

public class QuestListView : MonoBehaviour
{
    [SerializeField] ScrollableButtonList scrollableButtonList;
    [SerializeField] SpriteRenderer charaImage;

    public IObservable<int> OnButtonChanged => scrollableButtonList.OnButtonChanged;

    public void Init()
    {
        scrollableButtonList.Init(ButtonListType.Vertical, isActive: true);
    }

    public void SetCharaImage(Sprite charaSprite)
    {
        // TODO: マジックナンバー修正
        charaImage.DOFade(0f, 0f);
        charaImage.sprite = charaSprite;
        charaImage.DOFade(1f, 0.25f).SetDelay(0.1f);
    }
}
