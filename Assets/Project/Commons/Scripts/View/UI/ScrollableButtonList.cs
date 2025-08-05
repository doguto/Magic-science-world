using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Commons.Scripts.View.UI
{
    public class ScrollableButtonList : ButtonListBase
    {
        [SerializeField] float buttonInterval;
        
        protected override bool MoveNextFlag => buttonListType switch
        {
            ButtonListType.Vertical => Input.GetKeyDown(KeyCode.UpArrow),
            ButtonListType.Horizontal => Input.GetKeyDown(KeyCode.RightArrow),
            _ => false
        };
        
        protected override bool MoveBackFlag => buttonListType switch
        {
            ButtonListType.Vertical => Input.GetKeyDown(KeyCode.DownArrow),
            ButtonListType.Horizontal => Input.GetKeyDown(KeyCode.LeftArrow),
            _ => false
        };
        
        public override void MoveNext(bool isUp = true)
        {
            Debug.Log($"ScrollableButtonList: MoveNext: {isUp}");
            
            // 両端の場合はもう一端に行かずに移動不可にする
            if (ButtonIndex == buttons.Count - 1 && !isUp) return;
            if (ButtonIndex == 0 && isUp) return;
            
            buttons[ButtonIndex].SetActive(false);
            foreach (var button in buttons)
            {
                button.Move(new(0, isUp ? -buttonInterval : buttonInterval));
            }
            
            SetButtonIndex( isUp ? ButtonIndex - 1 : ButtonIndex + 1 );
            buttons[ButtonIndex].SetActive(true);
        }
    }
}