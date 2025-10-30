using Project.Commons.BossPhaseStateMachine.Scripts.Model;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.Commons.BossPhaseStateMachine.Scripts.Presenter
{
    public class SimplePhaseState: BossPhaseState
    {
        private readonly PlayableDirector timeline;
        private readonly BossPhaseState nextState;
        private readonly float hpThreshold;
        private bool hasTransitioned;

        public SimplePhaseState(PlayableDirector timeline, BossPhaseState nextState, float hpThreshold, BossPhaseStateMachine stateMachine) : base(stateMachine)
        {
            this.timeline = timeline;
            this.nextState = nextState;
            this.hpThreshold = hpThreshold;
        }
        
        public override void Enter()
        {
            timeline.Play();
            Debug.Log($"Timeline started: {timeline.name}");
        }

        public override void Update()
        {
            if (hasTransitioned) return;
            if (HealthModel.CurrentHp.Value <= hpThreshold) TriggerTransition();
        }
        
        public override void Exit()
        {
            timeline.Stop();
            Debug.Log($"Timeline stopped: {timeline.name}");
        }
        
        public void TriggerTransition()
        {
            if (hasTransitioned) return;
            Debug.Log($"Transition to {nextState.GetType().Name}");
            if (nextState != null)
            {
                StateMachine.TransitionTo(nextState);
                hasTransitioned = true;
            }
            else
            {
                Debug.Log("Final phase completed!");
                hasTransitioned = true;
            }
        }
    }
}