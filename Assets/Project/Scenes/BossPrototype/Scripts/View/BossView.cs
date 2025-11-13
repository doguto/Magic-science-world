using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UniRx;

namespace Project.Scenes.BossPrototype.Scripts.View
{
    public class BossView: MonoBehaviour
    {
        [SerializeField] private Transform[] phase1Points;
        [SerializeField] private Transform[] phase2Points;
        [SerializeField] private Transform[] phase3Points;
        [SerializeField] private float phase1MoveDuration;
        [SerializeField] private float phase2MoveDuration;
        [SerializeField] private float phase3MoveDuration;
        
        private Subject<BulletSpawnRequest> onBulletSpawnRequestSubject = new ();
        public IObservable<BulletSpawnRequest> OnBulletSpawnRequest => onBulletSpawnRequestSubject;
        
        private int phase1MoveCount;
        private int phase2MoveCount;
        private int phase3MoveCount;
        
        public void MoveToPoints(int phaseNum)
        {
            if (phaseNum is < 1 or > 3) return;
            
            Transform[] points = Array.Empty<Transform>();
            int moveCount = 0;
            float moveDuration = 0.5f;
            switch (phaseNum)
            {
                case 1:
                    points = phase1Points;
                    moveCount = phase1MoveCount;
                    moveDuration = phase1MoveDuration;
                    break;
                case 2:
                    points = phase2Points;
                    moveCount = phase2MoveCount;
                    moveDuration = phase2MoveDuration;
                    break;
                case 3:
                    points = phase3Points;
                    moveCount = phase3MoveCount;
                    moveDuration = phase3MoveDuration;
                    break;
            }
            if (points.Length == 0) Debug.LogError($"phaseNum: {phaseNum} Points is Empty. Check Inspector.");
            this.transform.DOMove(points[moveCount % points.Length].position, moveDuration);
            AddMoveCount(phaseNum);
        }

        private void AddMoveCount(int phaseNum)
        {
            switch(phaseNum)
            {
                case 1:
                    phase1MoveCount++;
                    break;
                case 2:
                    phase2MoveCount++;
                    break;
                case 3:
                    phase3MoveCount++;
                    break;
            }
        }

        public void RequestBulletSpawn(BulletSpawnRequest request)
        {
            onBulletSpawnRequestSubject.OnNext(request);
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