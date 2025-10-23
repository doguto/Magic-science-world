using Project.Scenes.StageList.Scripts.Model;
using Project.Scenes.StageList.Scripts.View;
using Project.Scripts.Presenter;
using UniRx;
using UnityEngine;

namespace Project.Scenes.StageList.Scripts.Presenter
{
    public class StageListScenePresenter : MonoPresenter
    {
        [SerializeField] StageCardListView stageCardListView;

        StageModel stageModel;

        void Awake()
        {
            stageModel = StageModelRepository.Get();
        }

        void Start()
        {
            stageCardListView.Init();
            ShowCharaImage(0);
            stageCardListView.OnButtonChanged.Subscribe(ShowCharaImage);
        }

        void ShowCharaImage(int buttonIndex)
        {
            var charaImage = stageModel.GetCharaImage(buttonIndex + 1);
            stageCardListView.SetCharaImage(charaImage);
        }
    }
}
