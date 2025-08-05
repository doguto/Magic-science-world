using UnityEngine;

namespace Project.Scenes.QuestList.Scripts.Presenter
{
    public class QuestListScenePresenter : MonoBehaviour
    {
        [SerializeField] QuestListView questListView;

        void Start()
        {
            questListView.Init();
        }
    }
}

