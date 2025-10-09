using System;
using Cysharp.Threading.Tasks;
using Project.Scenes.QuestList.Scripts.Model;
using Project.Scripts.Presenter;
using Project.Scripts.Repository;
using UniRx;
using UnityEngine;

namespace Project.Scenes.QuestList.Scripts.Presenter
{
    public class QuestListScenePresenter : MonoPresenter
    {
        [SerializeField] QuestListView questListView;

        StageModel stageModel;
        
        void Awake()
        {
            stageModel = StageModelRepository.Instance.Get();
        }

        void Start()
        {
            questListView.Init();
            ShowCharaImage(0);
            questListView.OnButtonChanged.Subscribe(ShowCharaImage);
        }

        void ShowCharaImage(int buttonIndex)
        {
            var charaImage = stageModel.GetCharaImage(buttonIndex + 1);
            questListView.SetCharaImage(charaImage);
        }
    }
}
