// Assets/Project/Scenes/BattleWay/Scripts/View/Pool/BulletPool.cs
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.View.Pool
{
    using Project.Scenes.BattleWay.Scripts.View;

    /// <summary>
    /// ごく簡単なオブジェクトプール（弾専用）
    /// </summary>
    public class BulletPool : MonoBehaviour
    {
        [Header("設定")]
        [SerializeField] private BulletView _bulletPrefab;
        [SerializeField, Tooltip("起動時に確保しておく個数")]
        private int _initialCapacity = 32;

        private readonly Queue<BulletView> _queue = new();

        private void Awake()
        {
            if (_bulletPrefab == null)
            {
                Debug.LogError("[BulletPool] Bullet Prefab が未設定です。");
                return;
            }

            for (int i = 0; i < _initialCapacity; i++)
            {
                var b = CreateInstance();
                Return(b);
            }
        }

        private BulletView CreateInstance()
        {
            var b = Instantiate(_bulletPrefab, transform);
            b.gameObject.SetActive(false);
            return b;
        }

        public BulletView Rent(Vector3 position, Quaternion rotation)
        {
            BulletView b = _queue.Count > 0 ? _queue.Dequeue() : CreateInstance();
            var t = b.transform;
            t.SetPositionAndRotation(position, rotation);
            b.gameObject.SetActive(true);
            return b;
        }

        public void Return(BulletView bullet)
        {
            if (bullet == null) return;
            bullet.gameObject.SetActive(false);
            bullet.transform.SetParent(transform, false);
            _queue.Enqueue(bullet);
        }
    }
}
