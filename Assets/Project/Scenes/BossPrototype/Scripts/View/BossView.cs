using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

namespace Project.Scenes.BossPrototype.Scripts.View
{
    public class BossView: MonoBehaviour
    {
        [SerializeField] private Transform[] points; 
        private int moveCount;
        // Qキー：1弾幕パターン（シンプル）の生成
        // Wキー：1弾幕パターン（時間経過で追加弾幕生成あり）の生成
        // 移動させる

        // void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.M))
        //     {
        //         MoveToPoints();
        //     }
        // }
        public void MoveToPoints()
        {
            Debug.Log("MoveToPoints");
            this.transform.DOMove(points[this.moveCount % points.Length].position, 0.5f);
            this.moveCount++;
        }
    }
}