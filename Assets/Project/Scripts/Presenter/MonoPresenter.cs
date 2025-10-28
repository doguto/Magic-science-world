using Project.Scenes.Global.Scripts.Presenter;
using Project.Scripts.Repository.ModelRepository;
using UnityEngine;

namespace Project.Scripts.Presenter
{
    public class MonoPresenter : MonoBehaviour
    {
        protected GlobalScenePresenter globalScenePresenter;

        // 代入だとMonoランタイムの起動前にStageModelRepositoryのコンストラクタが呼ばれてしまうので、
        // getterで static Instance を呼ぶ。
        protected StageModelRepository StageModelRepository => StageModelRepository.Instance;

        void Awake()
        {
            globalScenePresenter = FindFirstObjectByType<GlobalScenePresenter>();
            if (!globalScenePresenter)
            {
                Debug.LogWarning("GlobalScenePresenterが見つかりません", this);
            }
        }
    }
}
