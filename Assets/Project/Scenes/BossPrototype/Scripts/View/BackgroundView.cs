using UnityEngine;

namespace Project.Scenes.BossPrototype.Scripts.View
{
    public class BackgroundView: MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        public void Pause()
        {
            spriteRenderer.color = Color.gray;
        }

        public void Resume()
        {
            spriteRenderer.color = Color.white;
        }
    }
}