using System.Collections.Generic;
using Project.Commons.EnemyBulletPrototype.Scripts.Model;
using Project.Commons.EnemyBulletPrototype.Scripts.View;
using UnityEngine;
using UnityEngine.Pool;

namespace Project.Commons.EnemyBulletPrototype.Scripts.Presenter
{
    public class BulletManagerBase : MonoBehaviour
    {
        [SerializeField] private EnemyBulletViewBase simpleBulletPrefab;
        
        private ObjectPool<EnemyBulletViewBase> simpleBulletPool;
        private List<EnemyBulletPresenter> activeBullets = new();
        
        void Start()
        {
            InitializePool();
        }

        public void Cleanup()
        {
            foreach (var bullet in activeBullets) bullet.Dispose();
        }
        void InitializePool()
        {
            simpleBulletPool = new ObjectPool<EnemyBulletViewBase>(
                createFunc: () => Instantiate(simpleBulletPrefab),
                actionOnGet: bullet => bullet.gameObject.SetActive(true),
                actionOnRelease: bullet => bullet.gameObject.SetActive(false),
                actionOnDestroy: bullet => Destroy(bullet.gameObject),
                maxSize: 100
            );
        }
        
        // BulletSpawnSignalReceiverから呼ばれる
        public virtual void SpawnBullet(Vector2 position, Vector2 direction, float speed)
        {
            var view = simpleBulletPool.Get();
            view.transform.position = position;
            
            var presenter = new EnemyBulletPresenter();

            var model = new BulletModel(1, 1, direction, 10);
            
            presenter.Init(model, view, this); // 自分を渡す
            
            activeBullets.Add(presenter);
        }
        
        // EnemyBulletPresenterから呼ばれる
        public void ReturnBullet(EnemyBulletViewBase view, EnemyBulletPresenter presenter)
        {
            simpleBulletPool.Release(view);
            activeBullets.Remove(presenter);
        }
    }
}