using System;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.Commons.BossPrototype.Scripts.Presenter
{
    public class SimplePhaseState: BossPhaseState
    {
        private readonly PlayableDirector timeline;
        private readonly BossPhaseState nextState;
        private readonly float hpThreshold;
        private readonly float timeThreshold;
        private IDisposable phaseDisposable;
        
        public SimplePhaseState(PlayableDirector timeline, BossPhaseState nextState, float hpThreshold, float timeThreshold, BossPhaseStateMachine stateMachine) : base(stateMachine)
        {
            this.timeline = timeline;
            this.nextState = nextState;
            this.hpThreshold = hpThreshold;
            this.timeThreshold = timeThreshold;
        }
        
        public override void Enter()
        {
            timeline.gameObject.SetActive(true);
            timeline.extrapolationMode = DirectorWrapMode.Loop;
            timeline.Play();
            
            phaseDisposable = Observable.Merge(
                Observable.Timer(TimeSpan.FromSeconds(timeThreshold)).AsUnitObservable(),
                healthModel.CurrentHp.Where(hp => hp <= hpThreshold).AsUnitObservable()).Take(1).Subscribe(_ => TriggerTransition());
            Debug.Log($"Timeline started: {timeline.name}");
        }

        public override void Update()
        {

        }
        
        public override void Exit()
        {
            timeline.Stop();
            timeline.gameObject.SetActive(false);
            phaseDisposable?.Dispose();
            Debug.Log($"Timeline stopped: {timeline.name}");
        }
        
        private void TriggerTransition()
        {
            if (nextState != null)
            {
                Debug.Log($"Transition (Type: {nextState?.GetType().Name})");
                stateMachine.TransitionTo(nextState);
            }
            else
            {
                // シナリオへ
                this.Exit();
                Debug.Log("Boss Defeated!");
            }
        }
    }
}