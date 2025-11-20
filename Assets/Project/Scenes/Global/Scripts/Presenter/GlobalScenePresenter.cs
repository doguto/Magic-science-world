using Project.Scenes.Global.Scripts.View;
using UnityEngine;

namespace Project.Scenes.Global.Scripts.Presenter;

// GlobalScenePresenterはGlobalScenePresenterをFindすると変になるため、MonoPresenterを継承しない
public class GlobalScenePresenter : MonoBehaviour
{
    [SerializeField] SoundManagerView soundManagerView;
}
