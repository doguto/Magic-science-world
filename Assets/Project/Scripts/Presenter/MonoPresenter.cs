using Project.Scripts.Repository.ModelRepository;
using UnityEngine;

namespace Project.Scripts.Presenter
{
    public class MonoPresenter : MonoBehaviour
    {
        // 代入だとMonoランタイムの起動前にStageModelRepositoryのコンストラクタが呼ばれてしまうので、
        // getterで static Instance を呼ぶ。
        protected StageModelRepository StageModelRepository => StageModelRepository.Instance;
    }
}
