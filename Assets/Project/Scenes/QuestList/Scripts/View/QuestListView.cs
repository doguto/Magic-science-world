using Project.Commons.Scripts.View.UI;
using UnityEngine;

public class QuestListView : MonoBehaviour
{
    [SerializeField] ScrollableButtonList scrollableButtonList;


    public void Init()
    {
        scrollableButtonList.Init(ButtonListType.Vertical, isActive: true);
    }
}
