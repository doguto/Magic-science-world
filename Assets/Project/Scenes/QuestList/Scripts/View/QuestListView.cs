using System;
using Project.Commons.Scripts.View.UI;
using UnityEngine;
using UnityEngine.UI;

public class QuestListView : MonoBehaviour
{
    [SerializeField] ScrollableButtonList scrollableButtonList;
    [SerializeField] Image charaImage;

    public IObservable<int> OnButtonChanged => scrollableButtonList.OnButtonChanged;

    public void Init()
    {
        scrollableButtonList.Init(ButtonListType.Vertical, isActive: true);
    }

    public void SetCharaImage(Sprite charaSprite)
    {
        charaImage.gameObject.SetActive(true);
        charaImage.sprite = charaSprite;
    }
}
