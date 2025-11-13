// SignalReceiver
using UnityEngine;
using UnityEngine.Timeline;
using Project.Scenes.BossPrototype.Scripts.View;
using Project.Commons.EnemyBulletPrototype.Scripts.View;
using Project.Scripts.Infra;

namespace Project.Scenes.BossPrototype.Scripts.View
{
    public class BulletSpawnSignalReceiver : SignalReceiver
    {
        [SerializeField] private BossView _bossView;

        public void OnBulletSpawn(SignalAsset signal)
        {
            if (signal is BulletSpawnSignalAsset spawnSignal)
            {
                var request = new BulletSpawnRequest(transform.position, Vector2.left, spawnSignal.Speed);
                
                _bossView.RequestBulletSpawn(request);
            }
        }
    }

}