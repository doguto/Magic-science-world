using UnityEngine;
using TMPro;
using Project.Scenes.Scenario.Scripts.Model;

namespace Project.Scenes.Scenario.Scripts.View
{
  public class ScenarioView : MonoBehaviour
  {
      [SerializeField] private TextMeshProUGUI characterNameText;
      [SerializeField] private TextMeshProUGUI contentText;

      public void Display(ScenarioLine line)
      {
          characterNameText.text = line.character;
          contentText.text = line.content;
      }
  }
}
