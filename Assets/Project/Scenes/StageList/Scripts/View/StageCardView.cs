using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scenes.StageList.Scripts.View
{
    public class StageCardView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI stageTitle;
        [SerializeField] TextMeshProUGUI stageIndexText;
 
        public void Setup(Tuple<string, string> stage)
        {
            this.stageIndexText.text = $"Stage.{stage.Item1}";
            this.stageTitle.text = stage.Item2;
        }
    }    
}

