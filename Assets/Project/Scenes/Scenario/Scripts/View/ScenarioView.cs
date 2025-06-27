using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Project.Scenes.Scenario.Scripts.View
{
    public class ScenarioView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private Image faceImage;
        [SerializeField] private Image StillImageMe;
        [SerializeField] private Image StillImageEnemy;

        public void InitStillImage(string characterKeyMe, string characterKeyEnemy, string faceType)
        {
            SetStillImage(StillImageMe, characterKeyMe, faceType);
            SetStillImage(StillImageEnemy, characterKeyEnemy, faceType);
        }
        
        public class DisplayData
        {
            public string character { get; set; }
            public string content { get; set; }
            public string faceType { get; set; }
            public string characterKey { get; set; }
        }
        public void Display(DisplayData data)
        {
            characterNameText.text = data.character;
            contentText.text = data.content;
            UpdateFaceImage(data.characterKey, data.faceType);
        }

        private void UpdateFaceImage(string characterKey, string faceType)
        {
            string facePath = $"CharaImage/Face/{characterKey}/{faceType}";
            Sprite faceSprite = Resources.Load<Sprite>(facePath);
            if (faceSprite == null)
            {
                Debug.LogWarning($"faceSprite is null. Check the path: {facePath}");
                facePath = $"CharaImage/Face/{characterKey}/Default";
                faceSprite = Resources.Load<Sprite>(facePath);
            }
            faceImage.sprite = faceSprite;
        }

        private void SetStillImage(Image image, string characterKey, string faceType)
        {
            string fileName;
            if (faceType.Contains("Crazy"))
            {
                fileName = characterKey + "_Crazy";
            }
            else
            {
                fileName = characterKey;
            }
            string stillPath = $"CharaImage/Still/{fileName}";
            Sprite stillSprite = Resources.Load<Sprite>(stillPath);
            if (stillSprite == null)
            {
                Debug.LogWarning($"stillSprite is null. Check the path: {stillPath}");
            }
            image.sprite = stillSprite;
        }
    }
}