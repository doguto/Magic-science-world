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
                
            }
        }
    }
    public class BulletSpawnRequest
    {
        public BulletSpawnRequest(Vector2 position, Vector2 direction, float speed)
        {
            this.Position = position;
            this.Direction = direction;
            this.Speed = speed;
        }
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
    }
}