using System.Collections.Generic;
using UnityEngine;

namespace Project.Scenes.Battle.Scripts.View
{
    /// <summary>
    /// 攻撃の予告線。自分の transform.right 方向に点線を伸ばし、
    /// duration 経過後に自壊する。EnemySpawnAttackSignal で生成される想定のため、
    /// EnemyEntityPresenter は付けないこと(付けると EnemyTracker に敵として数えられる)。
    ///
    /// dotSprite 未設定でも実行時に白い正方形スプライトを生成して動くので、
    /// GameObject に本コンポーネントを付けるだけで予告線として成立する。
    /// </summary>
    public class WarningLineView : MonoBehaviour
    {
        [Header("Shape")]
        [SerializeField, Tooltip("未設定なら白い正方形を実行時生成する")]
        Sprite dotSprite;

        [SerializeField, Min(0f), Tooltip("線の長さ(ワールド単位)")]
        float length = 14f;

        [SerializeField, Min(0.01f), Tooltip("点の間隔(ワールド単位)")]
        float dotSpacing = 0.4f;

        [SerializeField, Min(0.01f), Tooltip("点1つの大きさ(ワールド単位)")]
        float dotSize = 0.12f;

        [SerializeField, Min(0f), Tooltip("原点から最初の点までの距離")]
        float startOffset = 0.5f;

        [Header("Appearance")]
        [SerializeField] Color color = new(1f, 0.92f, 0.25f, 0.75f);
        [SerializeField] string sortingLayerName = "Default";
        [SerializeField] int sortingOrder = 5;

        [Header("Animation")]
        [SerializeField, Min(0f), Tooltip("表示時間(秒)。0で自壊しない")]
        float duration = 1f;

        [SerializeField, Min(0f), Tooltip("点滅の周期(秒)。0で点滅なし")]
        float blinkInterval = 0.14f;

        [SerializeField, Tooltip("終盤で点滅を加速させる倍率。1で加速なし")]
        float blinkAccelerateRate = 3f;

        [SerializeField, Tooltip("点が奥へ流れる速度(ワールド単位/秒)。負で手前へ流れる")]
        float flowSpeed = 3f;

        [SerializeField, Min(0f), Tooltip("フェードインにかける時間(秒)")]
        float fadeInDuration = 0.15f;

        static Sprite fallbackDotSprite;

        readonly List<SpriteRenderer> dots = new();
        float elapsed;
        float blinkTimer;
        bool blinkOn = true;

        void Start()
        {
            BuildDots();
        }

        void BuildDots()
        {
            var sprite = dotSprite != null ? dotSprite : GetFallbackDotSprite();
            var count = Mathf.Max(1, Mathf.FloorToInt((length - startOffset) / dotSpacing) + 1);

            for (var i = 0; i < count; i++)
            {
                var dot = new GameObject($"Dot{i}");
                dot.transform.SetParent(transform, false);
                dot.transform.localPosition = new Vector3(startOffset + dotSpacing * i, 0f, 0f);

                var renderer = dot.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.color = color;
                renderer.sortingLayerName = sortingLayerName;
                renderer.sortingOrder = sortingOrder;
                // スプライトは 1x1 ワールド単位で生成しているので、localScale がそのまま点の大きさになる
                dot.transform.localScale = Vector3.one * dotSize;

                dots.Add(renderer);
            }
        }

        void Update()
        {
            elapsed += Time.deltaTime;

            UpdateBlink();
            UpdateFlow();
            ApplyAlpha();

            if (duration > 0f && elapsed >= duration) Destroy(gameObject);
        }

        void UpdateBlink()
        {
            if (blinkInterval <= 0f)
            {
                blinkOn = true;
                return;
            }

            // 残り時間が短いほど点滅を速くして「来るぞ」感を出す
            var progress = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 0f;
            var rate = Mathf.Lerp(1f, Mathf.Max(1f, blinkAccelerateRate), progress);
            var interval = blinkInterval / rate;

            blinkTimer += Time.deltaTime;
            if (blinkTimer < interval) return;

            blinkTimer -= interval;
            blinkOn = !blinkOn;
        }

        void UpdateFlow()
        {
            if (Mathf.Approximately(flowSpeed, 0f)) return;

            var span = dotSpacing * dots.Count;
            if (span <= 0f) return;

            for (var i = 0; i < dots.Count; i++)
            {
                var baseX = startOffset + dotSpacing * i;
                // 点線全体を dotSpacing 単位で循環させるので、見た目は「流れ続ける点線」になる
                var offset = Mathf.Repeat(flowSpeed * elapsed, dotSpacing);
                dots[i].transform.localPosition = new Vector3(baseX + offset, 0f, 0f);
            }
        }

        void ApplyAlpha()
        {
            var fade = fadeInDuration > 0f ? Mathf.Clamp01(elapsed / fadeInDuration) : 1f;
            var alpha = color.a * fade * (blinkOn ? 1f : 0.25f);
            var applied = new Color(color.r, color.g, color.b, alpha);

            foreach (var dot in dots)
            {
                if (dot != null) dot.color = applied;
            }
        }

        static Sprite GetFallbackDotSprite()
        {
            if (fallbackDotSprite != null) return fallbackDotSprite;

            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                hideFlags = HideFlags.HideAndDontSave
            };
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();

            // pixelsPerUnit=1 なので 1x1 テクスチャが 1x1 ワールド単位になる
            fallbackDotSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            fallbackDotSprite.hideFlags = HideFlags.HideAndDontSave;
            return fallbackDotSprite;
        }
    }
}
