using Project.Commons.BossPhaseStateMachine.Scripts.Model;
using UnityEngine;
using Project.Commons.BossPhaseStateMachine.Scripts.Presenter;

namespace Project.Scenes.BossPrototype.Scripts.Presenter
{
    public class BossController : MonoBehaviour
    {
        private BossPhaseStateMachine stateMachine;

        void Start()
        {
            stateMachine = GetComponent<BossPhaseStateMachine>();
            
            Debug.Log("=== Boss Debug Controller ===");
            Debug.Log("Space: Deal 10% damage");
            Debug.Log("D: Deal 30% damage");
            Debug.Log("K: Kill boss");
            Debug.Log("H: Show current HP");
        }

        void Update()
        {
            if (stateMachine?.HealthModel == null) return;
            
            // スペースキー: 10%ダメージ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float damage = stateMachine.HealthModel.MaxHp * 0.1f;
                stateMachine.HealthModel.TakeDamage(damage);
                Debug.Log($"[Debug] Dealt {damage} damage");
            }
            
            // Dキー: 30%ダメージ（フェーズ遷移テスト用）
            if (Input.GetKeyDown(KeyCode.D))
            {
                float damage = stateMachine.HealthModel.MaxHp * 0.3f;
                stateMachine.HealthModel.TakeDamage(damage);
                Debug.Log($"[Debug] Dealt {damage} damage");
            }
            
            // Kキー: 即死
            if (Input.GetKeyDown(KeyCode.K))
            {
                stateMachine.HealthModel.TakeDamage(stateMachine.HealthModel.MaxHp);
                Debug.Log($"[Debug] Boss killed!");
            }
            
            // Hキー: HP表示
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log($"[Debug] Current HP: {stateMachine.HealthModel.CurrentHp}");
            }
        }

    }
}