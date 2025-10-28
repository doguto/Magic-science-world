using Project.Commons.BossPhaseStateMachine.Scripts.Model;
using UnityEngine;

namespace Project.Commons.BossPhaseStateMachine.Scripts.Presenter
{
    public class BossPhaseStateMachine: MonoBehaviour
    {
        public BossHealthModel HealthModel { get; private set; }
        private BossPhaseState currentState;
        
        public void Init(BossHealthModel healthModel)
        {
            this.HealthModel = healthModel;
        }

        void Update()
        {
            currentState.Update();
        }

        public void TransitionTo(BossPhaseState newState)
        {
            if (currentState == null) return;
            
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
            
            Debug.Log($"Transition to {newState.GetType().Name}");
        }
    }
}