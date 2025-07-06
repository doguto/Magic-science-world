using TMPro;
using UnityEngine;

namespace Project.Scenes.QuestList.Scripts.View
{
    public class QuestCardView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI questTitle;
        [SerializeField] TextMeshProUGUI questIndexText;
 
        public void Setup(string questTitle, int questIndex)
        {
            this.questTitle.text = questTitle;
            this.questIndexText.text = $"Stage.{questIndex}";
        }
    }    
}

