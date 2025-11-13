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
 
        public void Setup((string id, string title)stage)
        {
            stageIndexText.text = $"Stage.{stage.id}";
            stageTitle.text = stage.title;
        }
    }
}
