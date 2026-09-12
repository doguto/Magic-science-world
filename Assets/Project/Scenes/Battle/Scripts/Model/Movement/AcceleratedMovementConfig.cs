using System;
using DG.Tweening;
using UnityEngine;

namespace Project.Scenes.Battle.Scripts.Model.Movement
{
    [Serializable]
    public class AcceleratedMovementConfig : IMovementStep
    {
        [SerializeField] Vector2 direction = Vector2.left;
        [SerializeField, Tooltip("ONでこの設定の Direction を優先し、攻撃側から渡された発射方向を使用しない。")]
        bool useConfiguredDirection = false;
        [SerializeField, Min(0f)] float initialSpeed = 2f;
        [SerializeField, Tooltip("負の値で減速")] float acceleration = 1f;
        [SerializeField, Min(0f)] float maxSpeed = 10f;
        [SerializeField, Min(0f)] float minSpeed = 0f;
        [SerializeField, Min(0f), Tooltip("継続時間（秒）。0で無限。")] float duration = 0f;
        [SerializeField, Tooltip("固定方向の加速度で放物運動する。ONの間は Acceleration / Max Speed / Min Speed を使用しない。")]
        bool useWorldAcceleration = false;
        [SerializeField, Tooltip("ワールド座標での加速度。Yを負にすると、発射方向に関係なく下に落ちる。")]
        Vector2 worldAcceleration = Vector2.down;

        public Tween Play(Transform target, Vector2 overrideDirection, Animator animator)
        {
            Vector3 dir = ((Vector3)(!useConfiguredDirection && overrideDirection != Vector2.zero
                ? overrideDirection : direction)).normalized;
            Vector3 velocity = dir * initialSpeed;

            if (useWorldAcceleration)
            {
                Vector3 gravity = worldAcceleration;
                return PullMovementHelper.Create(target, duration, (t, dt) =>
                {
                    t.position += velocity * dt + 0.5f * gravity * dt * dt;
                    velocity += gravity * dt;
                }, Ease.Linear);
            }

            float accel = acceleration;
            float max = maxSpeed;

            float min = minSpeed;

            return PullMovementHelper.Create(target, duration, (t, dt) =>
            {
                velocity += dir * accel * dt;
                float speed = velocity.magnitude;
                if (max > 0f && speed > max) velocity = velocity.normalized * max;
                if (speed < min) velocity = speed > 0f ? velocity.normalized * min : dir * min;
                t.position += velocity * dt;
            });
        }
    }
}
