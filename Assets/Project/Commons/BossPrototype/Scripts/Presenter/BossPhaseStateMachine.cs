using Project.Commons.BossPrototype.Scripts.Model;
using UnityEngine;

namespace Project.Commons.BossPrototype.Scripts.Presenter
{
    public class BossPhaseStateMachine: MonoBehaviour
    {
        public BossHealthModel HealthModel { get; private set; }
        private BossPhaseStateBase currentState;
        
        public void Init(BossHealthModel healthModel)
        {
            this.HealthModel = healthModel;
        }

        void Update()
        {
            if (currentState == null) return;
            currentState.Update();
        }

        public void TransitionTo(BossPhaseStateBase newState)
        {
            
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
            
            Debug.Log($"Transition (Type: {newState.GetType().Name})");
        }
    }
}