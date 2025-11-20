using Project.Commons.EnemyBulletPrototype.Scripts.Model;
using Project.Commons.EnemyBulletPrototype.Scripts.Presenter;
using UnityEngine;

namespace Project.Scenes.BossPrototype.Scripts.Presenter
{
    public class SimpleBulletManager: BulletManagerBase
    {
        public override void SpawnBullet(Vector2 position, Vector2 direction, float speed)
        {
            var view = simpleBulletPool.Get();
            view.transform.position = position;
            
            var presenter = new EnemyBulletPresenter();

            var model = new BulletModel(1, 1, direction, 10);
            
            presenter.Init(model, view, this); // 自分を渡す
            
            activeBullets.Add(presenter);
        }
    }
}
