using Project.Commons.BossPhaseStateMachine.Scripts.Model;
using UnityEngine.Playables;

namespace Project.Commons.BossPhaseStateMachine.Scripts.Presenter
{
    public class SimplePhaseState: BossPhaseState
    {
        private readonly PlayableDirector timeline;
        private readonly BossPhaseState nextState;

        public SimplePhaseState(PlayableDirector timeline, BossPhaseState nextState, BossPhaseStateMachine stateMachine) : base(stateMachine)
        {
            this.timeline = timeline;
            this.nextState = nextState;
        }
    }
}