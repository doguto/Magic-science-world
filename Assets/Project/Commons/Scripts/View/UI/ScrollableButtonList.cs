using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public class ScrollableButtonList : ButtonListBase
    {
        [SerializeField] float buttonInterval;
        
        public override void Init(ButtonListType buttonListType, int index = 0, bool isActive = false)
        {
            this.buttonListType = buttonListType;
            
            SetButtonIndex(index);
            buttons[ButtonIndex].SetActive(true);
            
            SetActive(isActive);
        }
        
        public override void MoveNext(bool isUp = true)
        {
            
        }
    }
}