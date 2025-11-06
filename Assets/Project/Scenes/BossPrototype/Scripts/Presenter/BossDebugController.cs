using Project.Commons.BossPrototype.Scripts.Model;
using UnityEngine;
using Project.Commons.BossPrototype.Scripts.Presenter;
using Project.Scenes.BossPrototype.Scripts.View;

namespace Project.Scenes.BossPrototype.Scripts.Presenter
{
    public class BossDebugController : MonoBehaviour
    {
        [SerializeField] BackgroundView backgroundView;
        
        private BossPhaseStateMachine stateMachine;
        private bool isPaused;

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
            
            // Escキー: ポーズ
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    backgroundView.Resume();
                    isPaused = false;
                    Time.timeScale = 1f;
                    Debug.Log("[Debug] Exit PauseMenu");
                }
                else
                {
                    backgroundView.Pause();
                    isPaused = true;
                    Time.timeScale = 0f;
                    Debug.Log("[Debug] Enter PauseMenu");
                }
            }
            if (isPaused) return;
            
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