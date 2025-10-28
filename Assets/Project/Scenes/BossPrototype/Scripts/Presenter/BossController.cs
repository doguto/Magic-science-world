using Project.Commons.BossPhaseStateMachine.Scripts.Model;
using UnityEngine;
using Project.Commons.BossPhaseStateMachine.Scripts.Presenter;

namespace Project.Scenes.BossPrototype.Scripts.Presenter
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private BossPhaseStateMachine stateMachine;
        void Update()
        {
            if (stateMachine?.HealthModel == null) return;
            
            // Kキーで即死
            if (Input.GetKeyDown(KeyCode.K))
            {
                stateMachine.HealthModel.TakeDamage(10);
                Debug.Log("Boss killed!");
            }
        }
    }
}