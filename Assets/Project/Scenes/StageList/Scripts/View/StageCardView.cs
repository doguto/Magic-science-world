using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scenes.StageList.Scripts.View
{
    public class StageCardView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI stageTitle;
        [SerializeField] TextMeshProUGUI stageIndexText;
 
        public void Setup(string stageTitle, int stageIndex)
        {
            this.stageTitle.text = stageTitle;
            this.stageIndexText.text = $"Stage.{stageIndex}";
        }
    }    
}

