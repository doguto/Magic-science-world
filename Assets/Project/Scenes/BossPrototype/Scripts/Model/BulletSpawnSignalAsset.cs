using UnityEngine;
using UnityEngine.Timeline;

namespace Project.Scenes.BossPrototype.Scripts.Model
{
    [CreateAssetMenu(fileName = "BulletSpawnSignal", menuName = "Signals/BulletSpawn")]
    public class BulletSpawnSignalAsset : SignalAsset
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
    }
}