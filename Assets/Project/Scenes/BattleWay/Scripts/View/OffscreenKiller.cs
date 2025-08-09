// Assets/Project/Scenes/BattleWay/Scripts/View/OffscreenKiller.cs
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.View
{
    /// <summary>
    /// 画面外に出たオブジェクトを自動で消す。
    /// ・参照カメラ：未指定なら Camera.main
    /// ・マージン：ビューポート外にどれだけはみ出たら消すか（-margin～1+margin）
    /// </summary>
    public class OffscreenKiller : MonoBehaviour
    {
        [Header("参照カメラ（未指定ならCamera.main）")]
        [SerializeField] private Camera _camera;

        [Header("消去判定のビューポート余白")]
        [SerializeField, Range(0f, 0.5f)] private float _margin = 0.1f;

        [Header("Update間隔（毎フレーム重いと感じたら上げる）")]
        [SerializeField, Min(0f)] private float _checkInterval = 0f;

        private float _timer;

        private void Reset()
        {
            // Bullet/Enemyなど動的オブジェクト想定なので既定は軽め
            _margin = 0.1f;
            _checkInterval = 0f;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_checkInterval > 0f && _timer < _checkInterval) return;
            _timer = 0f;

            var cam = _camera != null ? _camera : Camera.main;
            if (cam == null) return; // カメラ未設定なら何もしない

            Vector3 vp = cam.WorldToViewportPoint(transform.position);

            // カメラの前方にいない場合（Z<0）も消す
            bool outOfZ = vp.z < 0f;

            float min = -_margin;
            float max = 1f + _margin;
            bool outOfXY = (vp.x < min || vp.x > max || vp.y < min || vp.y > max);

            if (outOfZ || outOfXY)
            {
                DespawnOrDestroy();
            }
        }

        /// <summary>
        /// 現状はDestroy。後でプール返却に差し替え可能。
        /// </summary>
        private void DespawnOrDestroy()
        {
            // TODO: 弾のプール導入済みなら、ここで返却APIに差し替える
            Destroy(gameObject);
        }
    }
}
