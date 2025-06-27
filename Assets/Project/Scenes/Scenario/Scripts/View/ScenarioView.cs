using UnityEngine;
using TMPro;

namespace Project.Scenes.Scenario.Scripts.View
{
  public class ScenarioView : MonoBehaviour
  {
      [SerializeField] private TextMeshProUGUI characterNameText;
      [SerializeField] private TextMeshProUGUI contentText;

      public void Display(string character, string content)
      {
          characterNameText.text = character;
          contentText.text = content;
      }
  }
}
