# モデル実装マニュアル

Modelクラスの実装をする際の基本的なマニュアル

## 新規Model実装

### 1. Modelクラスの作成

Modelクラスは、データとそのデータに関連するロジックを保持する責務を持ちます。

#### 配置場所
- シーン固有のModel: `Assets/Project/Scenes/[シーン名]/Scripts/Model/`
- 共通のModel: `Assets/Project/Commons/Scripts/Model/`

#### 基本構造

```csharp
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Model;
using Project.Scripts.Infra;
using Project.Scripts.Repository.AssetRepository;
using UnityEngine;

namespace Project.Scenes.[シーン名].Scripts.Model
{
    public class [機能名]Model : ModelBase
    {
        // データのキャッシュ
        List<[機能名]Data> dataList;
        List<Sprite> cachedAssets = new();

        // コンストラクタでデータとアセットを初期化
        public [機能名]Model(List<[機能名]Data> dataList)
        {
            this.dataList = dataList;
            
            // AssetRepositoryを使用してアセットを読み込む
            var assetRepository = new SomeAssetRepository();
            cachedAssets = dataList
                .Select(x => assetRepository.Load(x.assetAddress))
                .ToList();
        }

        // データを取得するメソッド
        public Sprite GetAsset(int id)
        {
            return cachedAssets[id];
        }
    }
}
```

#### 重要なポイント
- `ModelBase`を継承する
- MonoBehaviourは継承**しない**（画面に依存しない層）
- データはコンストラクタで受け取る（ModelRepositoryから渡される）
- アセットの読み込みには`AssetRepository`を使用する
- 読み込んだアセットはキャッシュしておき、メソッドで提供する
- データに関連するロジックはModel内に記述する

#### アセット読み込みの例

ModelBase を直接使用する場合:

```csharp
public class ExampleModel : ModelBase
{
    public Sprite CharacterSprite { get; private set; }

    public ExampleModel(string characterAddress)
    {
        // 同期読み込み
        CharacterSprite = LoadAsset<Sprite>(characterAddress);
    }

    public async UniTask LoadCharacterAsync(string characterAddress)
    {
        // 非同期読み込み
        CharacterSprite = await LoadAssetAsync<Sprite>(characterAddress);
    }
}
```

AssetRepository を使用する場合（推奨）:

```csharp
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Model;
using Project.Scripts.Repository.AssetRepository;
using UnityEngine;

public class ExampleModel : ModelBase
{
    List<Sprite> characterSprites = new();

    public ExampleModel(List<string> characterAddresses)
    {
        var stillAssetRepository = new StillAssetRepository();
        characterSprites = characterAddresses
            .Select(address => stillAssetRepository.Load(address, false))
            .ToList();
    }

    public Sprite GetCharacterSprite(int index)
    {
        return characterSprites[index];
    }
}
```

### 2. データクラスの作成（必要に応じて）

Modelが扱うデータをScriptableObjectとして定義する場合の手順です。

#### 配置場所
- `Assets/Project/Scripts/Infra/`

#### ScriptableObjectデータの定義

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Infra
{
    // ScriptableObject本体
    [CreateAssetMenu(fileName = "[機能名]Data", menuName = "Database/[機能名]Data")]
    public class [機能名]DataObject : ScriptableObject
    {
        public List<[機能名]Data> data;
    }

    // データ構造
    [Serializable]
    public class [機能名]Data
    {
        public int id;
        public string name;
        public string description;
        // その他必要なフィールド
    }
}
```

#### 実装例：StageDataObject

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

#### 実装例：StageModel

StageDataを元にキャラクターの立ち絵を管理するModelの実装例です。

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

### 3. AssetRepositoryクラスの作成（必要に応じて）

AssetRepositoryは、アセットの読み込みロジックを一元管理するためのクラスです。
複雑なアドレス生成や、複数のModelで共通して使用するアセット読み込みロジックがある場合に作成します。

#### 配置場所
- `Assets/Project/Scripts/Repository/AssetRepository/`

#### 基本構造

```csharp
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Text;
using Project.Scripts.Extensions;

namespace Project.Scripts.Repository.AssetRepository
{
    public class [アセット種類]AssetRepository : AssetRepositoryBase
    {
        public Sprite Load(string assetName, bool option)
        {
            // アドレスの生成ロジック
            string address = ZString.Format(
                "{0}/[カテゴリ]/{1}/[サブパス]/{1}{2}.png",
                GamePath.TexturesPath,
                assetName,
                option ? "_Suffix" : ""
            );

            // Addressablesでアセットを読み込み
            Sprite asset = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion();
            return asset;
        }
    }
}
```

#### 実装例：StillAssetRepository

キャラクターの立ち絵を読み込むAssetRepositoryの実装例です。

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

#### 重要なポイント
- `AssetRepositoryBase`を継承する
- アドレス生成には`ZString.Format`を使用してメモリ効率を高める
- `GamePath`を使用してベースパスを取得する
- 複雑なアドレス生成ロジックを隠蔽し、Modelから簡単に使用できるようにする

### 4. ModelRepositoryクラスの作成

ModelRepositoryは、Modelインスタンスの生成と管理を行います。

#### 配置場所
- `Assets/Project/Scripts/Repository/ModelRepository/`

#### 基本構造

```csharp
using Project.Scripts.Infra;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Repository.ModelRepository
{
    public class [機能名]ModelRepository : ModelRepositoryBase
    {
        // シングルトンインスタンス
        public static [機能名]ModelRepository Instance { get; } = new();

        // データとModelのキャッシュ
        readonly List<[機能名]Data> dataList;
        [機能名]Model model;

        // コンストラクタでデータを読み込む
        public [機能名]ModelRepository()
        {
            dataName = "[機能名]Data";
            dataList = LoadData();
        }

        // Modelの取得（シングルトンパターン）
        public [機能名]Model Get()
        {
            if (model != null) return model;
            model = new [機能名]Model(dataList);
            return model;
        }

        // データの読み込み
        List<[機能名]Data> LoadData()
        {
            var dataObject = Addressables.LoadAssetAsync<[機能名]DataObject>(DataAddress).WaitForCompletion();
            return dataObject.data;
        }
    }
}
```

#### 重要なポイント
- `ModelRepositoryBase`を継承する
- シングルトンパターン`Instance`を実装する
- `dataName`を設定することで`DataAddress`が自動生成される
- Modelインスタンスをキャッシュし、再利用する
- データの読み込みはAddressablesを使用する

#### 実装例：StageModelRepository

実際のStageModelRepositoryの実装例です。

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

### 5. Presenterからの使用方法

```csharp
using Project.Scripts.Repository.ModelRepository;
using UnityEngine;

namespace Project.Scenes.[シーン名].Scripts.Presenter
{
    public class [シーン名]Presenter : MonoBehaviour
    {
        [機能名]Model model;

        void Start()
        {
            // Repositoryからモデルを取得
            model = [機能名]ModelRepository.Instance.Get();
            
            // モデルのデータを使用
            var asset = model.GetAsset(0);
        }
    }
}
```

#### 実装例：StageListPresenterでの使用

```csharp
using Project.Scripts.Repository.ModelRepository;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scenes.StageList.Scripts.Presenter
{
    public class StageListPresenter : MonoBehaviour
    {
        [SerializeField] Image charaImage;
        
        void Start()
        {
            // StageModelRepositoryからStageModelを取得
            var stageModel = StageModelRepository.Instance.Get();
            
            // ステージ1のキャラクター画像を取得して表示
            charaImage.sprite = stageModel.GetCharaImage(1);
        }
    }
}
```

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

`Assets/Project/Scenes/StageList/Scripts/Presenter/StageListPresenter.cs`

```csharp
using Project.Scripts.Repository.ModelRepository;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scenes.StageList.Scripts.Presenter
{
    public class StageListPresenter : MonoBehaviour
    {
        [SerializeField] Image charaImage;
        
        void Start()
        {
            var stageModel = StageModelRepository.Instance.Get();
            charaImage.sprite = stageModel.GetCharaImage(1);
        }
    }
}
```

## ベストプラクティス

### 命名規則
- Modelクラス: `[機能名]Model.cs`
- Repositoryクラス: `[機能名]ModelRepository.cs`
- AssetRepositoryクラス: `[アセット種類]AssetRepository.cs`
- DataObjectクラス: `[機能名]DataObject.cs`
- Dataクラス: `[機能名]Data`

### 設計原則
1. **単一責任の原則**: 一つのModelは一つの責務のみを持つ
2. **データとロジックの分離**: データ構造（Data）とビジネスロジック（Model）を分離
3. **依存性の管理**: ModelはViewやPresenterに依存しない
4. **再利用性**: Repositoryでインスタンスを管理し、再利用を促進

### チェックリスト
- [ ] Modelクラスは`ModelBase`を継承しているか
- [ ] Modelクラスは`MonoBehaviour`を継承していないか
- [ ] データはコンストラクタで受け取っているか
- [ ] アセット読み込みには`AssetRepository`を使用しているか（複雑なアドレス生成が必要な場合）
- [ ] Repositoryクラスは`ModelRepositoryBase`を継承しているか
- [ ] Repositoryクラスはシングルトンパターンを実装しているか
- [ ] データの読み込みはAddressablesを使用しているか
- [ ] 適切なネームスペースが設定されているか

## 参考資料
- [Architecture.md](./Architecture.md) - MVPアーキテクチャの詳細
- [Directory-Structure.md](./Directory-Structure.md) - ディレクトリ構造の説明
