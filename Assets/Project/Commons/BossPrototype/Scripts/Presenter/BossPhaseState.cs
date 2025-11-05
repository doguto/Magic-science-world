using Project.Commons.BossPrototype.Scripts.Model;

namespace Project.Commons.BossPrototype.Scripts.Presenter
{
    public abstract class BossPhaseState
    {
        protected BossHealthModel HealthModel;
        protected BossPhaseStateMachine StateMachine;
        
        protected BossPhaseState(BossPhaseStateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
            this.HealthModel = stateMachine.HealthModel;
        }
        
        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void Update() {}
    }
}