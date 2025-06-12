# Archietcture
魔科セカでのアーキテクチャについて解説を書く。

## Basis
基本的にはMVPというアーキテクチャに従う。MVPは、Model, View, Presenterの各頭文字を取ったものであり、これらを主要な構成要素とする。

### Model
Modelは永続化層、つまりはデータベースとのやり取りを主に行う。魔科セカではデータベースサーバーを立てたりはしないので、基本ScriptableObjectに格納したデータか、純粋にModel内に変数として定義したものを以後データと呼ぶ。

単にModelのデータを保存するだけではなく、データに関連するロジックはModel内に記述する。

またこの層はUnityの画面に依存しないことが望ましいため、MonoBehaviorを継承しないピュアクラスに記述する。

以下にサンプルのModelクラスを書く。
```cs
namespace Project.Scenes.SampleScene.Scripts.Model
{
  public class SampleModel : IDisposable
  {
    public int sampleAmount { get; private set; }
    public bool isCleared { get; private set; }

    publid void Dispose()
    {

    }

    public void Clear()
    {
      isCleared = true;
    }
  }
}
```


### View
Viewは画面の更新を主に行う。こちらはMonoBehaviorを継承し、基本的にはViewクラス一つに付き一つGameObjectを作成する。（逆にはならないので注意！ GameObject作ったらViewクラスを作らなければいけないわけではない。）

Unityに依存する層であり、SerializeFieldでその他のコンポーネントを持ち、操作する。

以下にサンプルのViewクラスを書く。
```cs
namespace Project.Scenes.SampleScene.Scripts.View
{
  public class SampleModel : IDisposable
  {
    [serializeField] ButtonList buttonList;

    public IObservable<UniRx.Unit> OnPressedStart => buttonList.GetButtonEvent(0);

    void Start()
    {
      buttonList.Init(0);
    }

    public void Clear()
    {
      isCleared = true;
    }

    public void SwitchSelectButton()
    {
        if (!buttonList.IsActive) return;
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) buttonList.MoveToNextButton();
        if (Input.GetKeyDown(KeyCode.DownArrow)) buttonList.MoveToNextButton(false);
    }

    public void SetActive(bool isActive)
    {
      gameObject.SetActive(isActive);
    }
  }
}
```

### Presenter
PresenterはModelとViewを用い、具体的なゲームのロジックを記述する。

基本的にはScene毎に一つ作成する。本来であれば画面に依存しない層のためMonoBehaviorを継承しないのだが、SerializeFieldでViewクラスのインスタンスを持ちたいため、例外的にMonoBehaviorを継承してGameObjectにアタッチする。

以下にサンプルのPresenterを書く。
```cs
namespace Project.Scenes.Title.Scripts.Presenter
{
    public class TitleScenePresenter : MonoBehaviour
    {
        [SerializeField] TitleMenuView titleMenuView;
        [SerializeField] TitleSettingModalView titleSettingModalView;
        
        TitleBackgroundModel _titleBackgroundModel;

        void Start()
        {
            _titleBackgroundModel = new TitleBackgroundModel();
            
            titleMenuView.OnPressedStart.Subscribe(StartGame);
            
            SetTitleBackGround();
        }

        void StartGame(Unit _)
        {
            
        }

        void SetTitleBackGround()
        {
            var clearedStageAmount = _titleBackgroundModel.ClearedStageAmount;
            titleMenuView.SetBackGround(clearedStageAmount);
        }
    }
}
```
