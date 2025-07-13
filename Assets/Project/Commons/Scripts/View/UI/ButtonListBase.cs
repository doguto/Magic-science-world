using System.Collections.Generic;
using UnityEngine;

namespace Project.Commons.Scripts.View.UI
{
    public abstract class ButtonListBase : MonoBehaviour
    {
        [SerializeField] protected List<ButtonBase> buttons;

        protected ButtonListType buttonListType;
        
        public int ButtonIndex { get; protected set; }
        public bool IsActive { get; protected set; }
        
        protected bool MoveNextFlag => buttonListType switch
        {
            ButtonListType.Vertical => Input.GetKeyDown(KeyCode.UpArrow),
            ButtonListType.Horizontal => Input.GetKeyDown(KeyCode.RightArrow),
            _ => false
        };
        
        protected bool MoveBackFlag => buttonListType switch
        {
            ButtonListType.Vertical => Input.GetKeyDown(KeyCode.DownArrow),
            ButtonListType.Horizontal => Input.GetKeyDown(KeyCode.LeftArrow),
            _ => false
        };

        public virtual void Init(ButtonListType buttonListType, int index = 0, bool isActive = false)
        {
            this.buttonListType = buttonListType;
            
            SetButtonIndex(index);
            buttons[ButtonIndex].SetActive(true);
            
            SetActive(isActive);
        }
        
        public void SetActive(bool active)
        {
            IsActive = active;
        }
        
        protected void SetButtonIndex(int index)
        {
            ButtonIndex = (index + buttons.Count) % buttons.Count;
        }
    }
    
    public enum ButtonListType
    {
        Vertical,
        Horizontal,
    }
}