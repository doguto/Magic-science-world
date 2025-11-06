using Project.Commons.BossPrototype.Scripts.Presenter;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.Scenes.BossPrototype.Scripts.Presenter
{
    public class BossPrototypePhaseController : MonoBehaviour
    {
        [SerializeField] private PlayableDirector phase1Timeline;
        [SerializeField] private PlayableDirector phase2Timeline;
        [SerializeField] private PlayableDirector phase3Timeline;
        
        [SerializeField] private BossPhaseStateMachine stateMachine;
        [SerializeField] private BossPresenter bossPresenter;
        
        void Start()
        {
            // 初期化確認
            if (stateMachine == null)
            {
                Debug.LogError("[Boss1Controller] BossPhaseStateMachine not found!");
                return;
            }
            
            if (stateMachine.HealthModel == null)
            {
                Debug.LogError("[Boss1Controller] HealthModel is not initialized!");
                return;
            }
            InitializePhases();
        }

        private void InitializePhases()
        {
            var phase3 = new SimplePhaseState(phase3Timeline, null, 0.0f, 10f, stateMachine);
            var phase2 = new SimplePhaseState(phase2Timeline, phase3, 200f, 10f, stateMachine);
            var phase1 = new SimplePhaseState(phase1Timeline, phase2, 500f, 10f, stateMachine);
            stateMachine.TransitionTo(phase1);
        }
    }
}