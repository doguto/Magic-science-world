using Project.Commons.BossPrototype.Scripts.Model;

namespace Project.Commons.BossPrototype.Scripts.Presenter
{
    public abstract class BossPhaseStateBase
    {
        protected BossHealthModel healthModel;
        protected BossPhaseStateMachine stateMachine;
        
        protected BossPhaseStateBase(BossPhaseStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            this.healthModel = stateMachine.HealthModel;
        }
        
        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void Update() {}
    }
}