using Cysharp.Threading.Tasks;
using Project.Scenes.QuestList.Scripts.Model;
using UniRx;
using UnityEngine;

namespace Project.Scenes.QuestList.Scripts.Presenter
{
    public class QuestListScenePresenter : MonoBehaviour
    {
        [SerializeField] QuestListView questListView;

        QuestModel questModel;
        
        void Awake()
        {
            questModel = new QuestModel();
        }
        
        void Start()
        {
            questListView.Init();
            ShowCharaImage(0);
            questListView.OnButtonChanged.Subscribe(ShowCharaImage);
        }

        void ShowCharaImage(int buttonIndex)
        {
            var charaImage = questModel.GetCharaImage(buttonIndex + 1);
            questListView.SetCharaImage(charaImage);
        }
    }
}
