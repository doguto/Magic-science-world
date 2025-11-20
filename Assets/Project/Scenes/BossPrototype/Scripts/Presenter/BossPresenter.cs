using UnityEngine;
using UniRx;
using Project.Commons.BossPrototype.Scripts.Model;
using Project.Commons.BossPrototype.Scripts.Presenter;
using Project.Commons.EnemyBulletPrototype.Scripts.Presenter;
using Project.Commons.EnemyBulletPrototype.Scripts.Model;
using Project.Scenes.BossPrototype.Scripts.View;

namespace Project.Scenes.BossPrototype.Scripts.Presenter
{
    public class BossPresenter : MonoBehaviour
    {
        [SerializeField] private float maxHP = 1000f;
        [SerializeField] private BossView view;
        [SerializeField] private BulletManagerBase bulletManager;
        
        private BossHealthModel healthModel;
        private BossPhaseStateMachine stateMachine;
        private BulletModel bulletModel;    
        
        void Awake()
        {
            // Model作成
            healthModel = new BossHealthModel(maxHP);
            
            // StateMachine初期化
            stateMachine = GetComponent<BossPhaseStateMachine>();
            stateMachine.Init(healthModel);
            
            Debug.Log($"[BossPresenter] Initialized with HP: {maxHP}");
        }

        void Start()
        {
            // HP変化をログ出力（Viewの代わり）
            healthModel.CurrentHp
                .Subscribe(hp => 
                {
                    Debug.Log($"[BossPresenter] HP: {hp:F0}/{maxHP} ");
                    
                    if (healthModel.IsDead)
                    {
                        Debug.Log($"[BossPresenter] Boss Defeated!");
                    }
                })
                .AddTo(this);
        }
        
        public void OnPhaseChanged(int phaseNumber)
        {
            Debug.Log($"[BossPresenter] Phase Changed: Phase {phaseNumber}");
        }

        void OnDestroy()
        {
            healthModel?.Dispose();
        }
    }
}