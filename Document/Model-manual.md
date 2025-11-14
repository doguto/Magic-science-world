# モデル実装マニュアル

Modelクラスの実装をする際の基本的なマニュアル

このマニュアルでは、実際のプロジェクトで使用されているStageModelを例に、Modelの実装方法を解説します。

## 新規Model実装

### 1. Modelクラスの作成

StageModelは、ステージリスト画面でステージ情報とキャラクターの立ち絵を管理するModelです。

#### 配置場所
- シーン固有のModel: `Assets/Project/Scenes/[シーン名]/Scripts/Model/`
- 共通のModel: `Assets/Project/Commons/Scripts/Model/`

StageModelの場合: `Assets/Project/Scenes/StageList/Scripts/Model/StageModel.cs`

#### StageModelの実装

```csharp
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
        List<Sprite> charaImages = new();

        public StageModel(List<StageData> stages)
        {
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

#### 重要なポイント
- `ModelBase`を継承する
- MonoBehaviourは継承**しない**（画面に依存しない層）
- データはコンストラクタで受け取る（ModelRepositoryから渡される）
- コンストラクタで受け取ったデータは、必要な情報を抽出した後は保持しない（メモリ効率のため）
- アセットの読み込みには`AssetRepository`を使用する
- 読み込んだアセットはキャッシュしておき、メソッドで提供する（`charaImages`）
- データに関連するロジックはModel内に記述する（`GetCharaImage`メソッド）

#### アセット読み込みの参考

StageModelでは`AssetRepository`を使用してアセットを読み込んでいます。
別のパターンとして、ModelBaseを直接使用することもできます：

```csharp
public class TitleBackgroundModel : ModelBase
{
    public Sprite BackgroundSprite { get; private set; }

    public TitleBackgroundModel(string backgroundAddress)
    {
        // ModelBaseの同期読み込みメソッドを使用
        BackgroundSprite = LoadAsset<Sprite>(backgroundAddress);
    }

    public async UniTask LoadBackgroundAsync(string backgroundAddress)
    {
        // ModelBaseの非同期読み込みメソッドを使用
        BackgroundSprite = await LoadAssetAsync<Sprite>(backgroundAddress);
    }
}
```

ただし、StageModelのように複雑なアドレス生成が必要な場合は、AssetRepositoryを使用することを推奨します。

### 2. データクラスの作成（必要に応じて）

StageModelが扱うデータをScriptableObjectとして定義します。

#### 配置場所
- `Assets/Project/Scripts/Infra/`

StageDataの場合: `Assets/Project/Scripts/Infra/StageDataObject.cs`

#### StageDataObjectの実装

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Infra
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Database/StageData")]
    public class StageDataObject : ScriptableObject
    {
        public List<StageData> stageData;
    }

    [Serializable]
    public class StageData
    {
        public int id;
        public string charaStillAddress;
    }
}
```

#### 解説

- `StageDataObject`: Unity上で作成・編集できるScriptableObjectのクラス
  - `[CreateAssetMenu]`属性で、Unityエディタのメニューから作成可能にする
  - `stageData`リストでステージ情報を保持

- `StageData`: 各ステージの情報を表すデータクラス
  - `id`: ステージID
  - `charaStillAddress`: キャラクターの立ち絵のアドレス（アセット名）
  - `[Serializable]`属性でUnityエディタで編集可能にする

### 3. AssetRepositoryクラスの作成（必要に応じて）

StageModelでは、キャラクターの立ち絵を読み込むために`StillAssetRepository`を使用しています。
AssetRepositoryは、複雑なアドレス生成ロジックを持つアセットの読み込みを一元管理するためのクラスです。

#### 配置場所
- `Assets/Project/Scripts/Repository/AssetRepository/`

StillAssetRepositoryの場合: `Assets/Project/Scripts/Repository/AssetRepository/StillAssetRepository.cs`

#### StillAssetRepositoryの実装

```csharp
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Text;
using Project.Scripts.Extensions;

namespace Project.Scripts.Repository.AssetRepository
{
    public class StillAssetRepository : AssetRepositoryBase
    {
        public Sprite Load(string charaName, bool isCrazy)
        {
            string address = ZString.Format(
                "{0}/Character/{1}/Still/{1}{2}_Still.png",
                GamePath.TexturesPath,
                charaName,
                isCrazy ? "_Crazy" : ""
            );

            Sprite asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}
```

#### 解説

- `AssetRepositoryBase`を継承する
- `Load`メソッドで、キャラクター名(`charaName`)と状態フラグ(`isCrazy`)からアドレスを生成
- `ZString.Format`を使用してメモリ効率よくアドレスを生成
- `GamePath.TexturesPath`でテクスチャのベースパスを取得
- 例: `charaName="Alice"`, `isCrazy=false` の場合
  - → `"Textures/Character/Alice/Still/Alice_Still.png"`
- Addressablesで同期的にアセットを読み込んで返す

このように、複雑なアドレス生成ロジックを隠蔽することで、StageModelから簡単に使用できるようになります。

### 4. ModelRepositoryクラスの作成

StageModelRepositoryは、StageModelインスタンスの生成と管理を行います。

#### 配置場所
- `Assets/Project/Scripts/Repository/ModelRepository/`

StageModelRepositoryの場合: `Assets/Project/Scripts/Repository/ModelRepository/StageModelRepository.cs`

#### StageModelRepositoryの実装

```csharp
using System.Collections.Generic;
using Project.Scenes.StageList.Scripts.Model;
using Project.Scripts.Infra;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Repository.ModelRepository
{
    public class StageModelRepository : ModelRepositoryBase
    {
        public static StageModelRepository Instance { get; } = new();
        
        readonly List<StageData> stages;
        StageModel stageModel;

        public StageModelRepository()
        {
            dataName = "StageData";
            stages = LoadData();
        }

        // Get()はModel毎に処理が変わる。
        // 引数でint idを取ったりするものもあるかも
        public StageModel Get()
        {
            if (stageModel != null) return stageModel;
            stageModel = new StageModel(stages);
            return stageModel;
        }

        List<StageData> LoadData()
        {
            var dataObject = Addressables.LoadAssetAsync<StageDataObject>(DataAddress).WaitForCompletion();
            return dataObject.stageData;
        }
    }
}
```

#### 解説

- `ModelRepositoryBase`を継承する
- `Instance`プロパティでシングルトンパターンを実装
  - アプリ全体で1つのインスタンスのみを使用する
- コンストラクタで`dataName`を設定し、データを読み込む
  - `dataName = "StageData"`を設定すると、`DataAddress`が自動生成される
  - `LoadData()`でAddressablesを使用してStageDataObjectを読み込む
- `Get()`メソッドでStageModelを取得
  - 初回呼び出し時にStageModelインスタンスを生成
  - 2回目以降はキャッシュされたインスタンスを返す（シングルトン）
- `LoadData()`でStageDataObjectから実際のデータリストを取得

### 5. Presenterからの使用方法

ステージリスト画面のPresenterで、StageModelRepositoryを使ってStageModelを取得し、ステージ情報を表示します。

#### StageListScenePresenterでの使用例

```csharp
using Project.Scenes.StageList.Scripts.Model;
using Project.Scenes.StageList.Scripts.View;
using Project.Scripts.Presenter;
using UniRx;
using UnityEngine;

namespace Project.Scenes.StageList.Scripts.Presenter
{
    public class StageListScenePresenter : MonoPresenter
    {
        [SerializeField] StageCardListView stageCardListView;

        StageModel stageModel;

        void Awake()
        {
            stageModel = StageModelRepository.Get();
        }

        void Start()
        {
            stageCardListView.Init();
            ShowCharaImage(0);
            stageCardListView.OnButtonChanged.Subscribe(ShowCharaImage);
        }

        void ShowCharaImage(int buttonIndex)
        {
            var charaImage = stageModel.GetCharaImage(buttonIndex + 1);
            stageCardListView.SetCharaImage(charaImage);
        }
    }
}
```

#### 解説

1. **MonoPresenterを継承**
   - `MonoPresenter`を継承することで、protectedプロパティ`StageModelRepository`にアクセス可能
   - `MonoPresenter`内で`protected StageModelRepository StageModelRepository => StageModelRepository.Instance;`が定義されている
   
2. **Awake()でModelを取得**
   - `StageModelRepository.Get()`でStageModelを取得
   - `StageModelRepository`はMonoPresenterのprotectedプロパティで、内部的に`StageModelRepository.Instance`を返す
   - Repository namespaceのimportは不要（MonoPresenterが提供するプロパティを使用）

3. **Modelのメソッドを使用**
   - `stageModel.GetCharaImage(buttonIndex + 1)`でキャラクター画像を取得
   - 取得した画像をViewに設定して表示

このように、PresenterはMonoPresenterを継承することでModelRepositoryへの便利なアクセス方法を得られ、Modelが提供するメソッドを使ってデータを取得します。

## 実装フロー全体例

StageModelを例にした、データ定義からPresenterでの使用までの完全な実装フローです。

### ステップ1: データクラスの作成

`Assets/Project/Scripts/Infra/StageDataObject.cs`

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Infra
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Database/StageData")]
    public class StageDataObject : ScriptableObject
    {
        public List<StageData> stageData;
    }

    [Serializable]
    public class StageData
    {
        public int id;
        public string charaStillAddress;
    }
}
```

### ステップ2: AssetRepositoryの作成（必要な場合）

`Assets/Project/Scripts/Repository/AssetRepository/StillAssetRepository.cs`

```csharp
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Text;
using Project.Scripts.Extensions;

namespace Project.Scripts.Repository.AssetRepository
{
    public class StillAssetRepository : AssetRepositoryBase
    {
        public Sprite Load(string charaName, bool isCrazy)
        {
            string address = ZString.Format(
                "{0}/Character/{1}/Still/{1}{2}_Still.png",
                GamePath.TexturesPath,
                charaName,
                isCrazy ? "_Crazy" : ""
            );

            Sprite asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}
```

### ステップ3: Modelクラスの作成

`Assets/Project/Scenes/StageList/Scripts/Model/StageModel.cs`

```csharp
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
        List<Sprite> charaImages = new();

        public StageModel(List<StageData> stages)
        {
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

### ステップ4: ModelRepositoryの作成

`Assets/Project/Scripts/Repository/ModelRepository/StageModelRepository.cs`

```csharp
using System.Collections.Generic;
using Project.Scenes.StageList.Scripts.Model;
using Project.Scripts.Infra;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Repository.ModelRepository
{
    public class StageModelRepository : ModelRepositoryBase
    {
        public static StageModelRepository Instance { get; } = new();
        
        readonly List<StageData> stages;
        StageModel stageModel;

        public StageModelRepository()
        {
            dataName = "StageData";
            stages = LoadData();
        }

        public StageModel Get()
        {
            if (stageModel != null) return stageModel;
            stageModel = new StageModel(stages);
            return stageModel;
        }

        List<StageData> LoadData()
        {
            var dataObject = Addressables.LoadAssetAsync<StageDataObject>(DataAddress).WaitForCompletion();
            return dataObject.stageData;
        }
    }
}
```

### ステップ5: Presenterでの使用

`Assets/Project/Scenes/StageList/Scripts/Presenter/StageListScenePresenter.cs`

```csharp
using Project.Scenes.StageList.Scripts.Model;
using Project.Scenes.StageList.Scripts.View;
using Project.Scripts.Presenter;
using UniRx;
using UnityEngine;

namespace Project.Scenes.StageList.Scripts.Presenter
{
    public class StageListScenePresenter : MonoPresenter
    {
        [SerializeField] StageCardListView stageCardListView;

        StageModel stageModel;

        void Awake()
        {
            stageModel = StageModelRepository.Get();
        }

        void Start()
        {
            stageCardListView.Init();
            ShowCharaImage(0);
            stageCardListView.OnButtonChanged.Subscribe(ShowCharaImage);
        }

        void ShowCharaImage(int buttonIndex)
        {
            var charaImage = stageModel.GetCharaImage(buttonIndex + 1);
            stageCardListView.SetCharaImage(charaImage);
        }
    }
}
```

## ベストプラクティス

### 命名規則

StageModelを例にした命名規則です：

- Modelクラス: `StageModel.cs`
- Repositoryクラス: `StageModelRepository.cs`
- AssetRepositoryクラス: `StillAssetRepository.cs`
- DataObjectクラス: `StageDataObject.cs`
- Dataクラス: `StageData`

基本パターン: `[機能名]Model.cs`, `[機能名]ModelRepository.cs`, `[機能名]DataObject.cs`

### 設計原則
1. **単一責任の原則**: 一つのModelは一つの責務のみを持つ
2. **データとロジックの分離**: データ構造（Data）とビジネスロジック（Model）を分離
3. **依存性の管理**: ModelはViewやPresenterに依存しない
4. **再利用性**: Repositoryでインスタンスを管理し、再利用を促進
5. **MonoPresenterの活用**: PresenterはMonoPresenterを継承し、protectedプロパティ経由でRepositoryにアクセス

### Presenterでの注意点

#### MonoPresenterを使用する場合（推奨）
```csharp
public class YourPresenter : MonoPresenter
{
    void Awake()
    {
        // MonoPresenterのprotectedプロパティを使用
        var model = StageModelRepository.Get();
    }
}
```

#### MonoPresenterを使用しない場合
```csharp
public class YourPresenter : MonoBehaviour
{
    void Start()
    {
        // 直接Instance経由でアクセス
        var model = StageModelRepository.Instance.Get();
    }
}
```

MonoPresenterには以下のコメントがあります：
```csharp
// 代入だとMonoランタイムの起動前にStageModelRepositoryのコンストラクタが呼ばれてしまうので、
// getterで static Instance を呼ぶ。
protected StageModelRepository StageModelRepository => StageModelRepository.Instance;
```

### チェックリスト
- [ ] Modelクラスは`ModelBase`を継承しているか
- [ ] Modelクラスは`MonoBehaviour`を継承していないか
- [ ] データはコンストラクタで受け取っているか
- [ ] アセット読み込みには`AssetRepository`を使用しているか（複雑なアドレス生成が必要な場合）
- [ ] Repositoryクラスは`ModelRepositoryBase`を継承しているか
- [ ] Repositoryクラスはシングルトンパターンを実装しているか
- [ ] データの読み込みはAddressablesを使用しているか
- [ ] Presenterは`MonoPresenter`を継承しているか
- [ ] 適切なネームスペースが設定されているか

## 参考資料
- [Architecture.md](./Architecture.md) - MVPアーキテクチャの詳細
- [Directory-Structure.md](./Directory-Structure.md) - ディレクトリ構造の説明
