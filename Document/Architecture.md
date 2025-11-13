# Architecture
魔科セカでのアーキテクチャについて解説を書く。

## Basis
基本的にはMVPというアーキテクチャに従う。MVPは、Model, View, Presenterの各頭文字を取ったものであり、これらを主要な構成要素とする。

### Model
Modelは永続化層、つまりはデータベースとのやり取りを主に行う。
魔科セカではデータベースサーバーを立てたりはしないので、基本ScriptableObjectに格納したデータか、純粋にModel内に変数として定義したものを以後データと呼ぶ。

単にModelのデータを保存するだけではなく、データに関連するロジックはModel内に記述する。

またこの層はUnityの画面に依存しないことが望ましいため、MonoBehaviorを継承しないピュアクラスに記述する。

以下にサンプルのModelクラスを書く。
```cs
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Model;
using Project.Scripts.Infra;
using Project.Scripts.Repository.AssetRepository;
using UnityEngine;

namespace Project.Scenes.StageList.Scripts.Model
{
    public class StageModel : ModelBase
    {
        List<StageData> stages;
        List<Sprite> charaImages = new();

         public StageModel(List<StageData> stages)
         {
             this.stages = stages;

             var stillAssetRepository = new StillAssetRepository();
             charaImages = stages
                 .Select(x => stillAssetRepository.Load(x.charaStillAddress, false))
                 .ToList();
         }

         public Sprite GetCharaImage(int stageNumber)
         {
             // stageNumberは1からなので1引いて合わせる
             var sprite = charaImages[stageNumber - 1];
             return sprite;
         }
    }
}
```

このようにModelはデータの取得や操作を行う関数をpublicに持つ。これらの関数は後述するPresenterから使用される。

このようにModelクラスを用意する理由は、Unityに依存するクラスはインスタンスの扱いが非常に面倒という点がある。
MonoBehaviorを継承したクラスは基本的にインスタンスとGameObjectとが密結合してしまうため、別クラスから気軽に取得することが出来ない。データを操作するクラスは一貫したインスタンスを様々な箇所から気軽に取得できて欲しいため、ピュアC#クラスとして切り出して実装する。

また単純に単一責任の原則から、データの操作をするクラスを切り出したいという要求もある。
PresenterやViewで上記のようなデータを持つことは処理が散らばってよろしくない。このような観点から、Modelクラスを実装する。


### View
Viewは画面の更新を主に行う。こちらはMonoBehaviorを継承し、基本的にはViewクラス一つに付き一つGameObjectを作成する。（逆にはならないので注意！ GameObject作ったらViewクラスを作らなければいけないわけではない。）

Unityに依存する層であり、SerializeFieldでその他のコンポーネントを持ち、操作する。

以下にサンプルのViewクラスを書く。
```cs
namespace Project.Scenes.SampleScene.Scripts.View
{
  public class SampleView : MonoBehaviour
  {
    [SerializeField] ButtonList buttonList;

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
