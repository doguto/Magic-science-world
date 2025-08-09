// Assets/Project/Scenes/BattleWay/Scripts/View/HitDispatcher.cs
using System;
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.View
{
    /// <summary>
    /// 2Dトリガー衝突をPresenterへ通知するための薄い入り口。
    /// - 対象は Collider2D の isTrigger = true を想定
    /// - レイヤーマトリクスの設定は別で行うこと
    /// </summary>
    public class HitDispatcher : MonoBehaviour
    {
        [Serializable]
        public struct HitEvent
        {
            public GameObject self;
            public GameObject other;
            public string selfTag;
            public string otherTag;
        }

        /// <summary>
        /// 衝突通知（購読はPresenter側で）
        /// </summary>
        public event Action<HitEvent> OnHit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var e = new HitEvent
            {
                self = this.gameObject,
                other = other.gameObject,
                selfTag = this.tag,
                otherTag = other.gameObject.tag
            };
            OnHit?.Invoke(e);
        }
    }
}
