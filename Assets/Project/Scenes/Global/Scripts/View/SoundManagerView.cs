using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Project.Scripts.Extensions;
using UnityEngine;

namespace Project.Scenes.Global.Scripts.View;

public class SoundManagerView : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public UniTask PlayBGM(SceneType sceneType, BgmType bgmType = BgmType.Default)
    {
        audioSource.Play();
        return UniTask.CompletedTask;
    }

    string CreateAddress(SceneType sceneType, BgmType bgmType)
    {
        return ZString.Format(
            "{0}/Sounds/BGM/{1}_{2}",
            GamePath.TexturesPath,
            sceneType.ToString(),
            bgmType.ToString()
        );
    }
}
