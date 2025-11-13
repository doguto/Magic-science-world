// SignalReceiver

using Project.Commons.EnemyBulletPrototype.Scripts.Presenter;
using UnityEngine;
using UnityEngine.Timeline;
using Project.Scenes.BossPrototype.Scripts.View;
using Project.Scenes.BossPrototype.Scripts.Model;

namespace Project.Scenes.BossPrototype.Scripts.Presenter
{
    public class BulletSpawnSignalReceiver : SignalReceiver
    {
        [SerializeField] private BulletManager bulletManager;

        public void OnBulletSpawn(SignalAsset signal)
        {
            if (signal is BulletSpawnSignalAsset spawnSignal)
            {
                bulletManager.SpawnBullet(transform.position, spawnSignal.Direction, spawnSignal.Speed);
            }
        }
    }
}