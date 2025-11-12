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
using Project.Scripts.Model;

namespace Project.Scenes.[シーン名].Scripts.Model
{
    public class [機能名]Model : ModelBase
    {
        // プロパティ（外部から読み取り専用）
        public int SomeValue { get; private set; }
        public bool IsActive { get; private set; }

        // コンストラクタ（必要に応じて初期化処理）
        public [機能名]Model()
        {
            SomeValue = 0;
            IsActive = false;
        }

        // ビジネスロジック
        public void DoSomething()
        {
            IsActive = true;
            SomeValue++;
        }
    }
}
```

#### 重要なポイント
- `ModelBase`を継承する
- MonoBehaviourは継承**しない**（画面に依存しない層）
- プロパティは`{ get; private set; }`で外部から読み取り専用にする
- データに関連するロジックはModel内に記述する
- アセットやデータの読み込みには`ModelBase`のメソッドを活用する

#### アセット読み込みの例

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
```

### 3. ModelRepositoryクラスの作成

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

```csharp
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
```

### 4. Presenterからの使用方法

```csharp
using Project.Scripts.Repository.ModelRepository;

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
            var value = model.SomeValue;
        }
    }
}
```

## ベストプラクティス

### 命名規則
- Modelクラス: `[機能名]Model.cs`
- Repositoryクラス: `[機能名]ModelRepository.cs`
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
- [ ] プロパティは適切にカプセル化されているか（`{ get; private set; }`）
- [ ] Repositoryクラスは`ModelRepositoryBase`を継承しているか
- [ ] Repositoryクラスはシングルトンパターンを実装しているか
- [ ] データの読み込みはAddressablesを使用しているか
- [ ] 適切なネームスペースが設定されているか

## 参考資料
- [Architecture.md](./Architecture.md) - MVPアーキテクチャの詳細
- [Directory-Structure.md](./Directory-Structure.md) - ディレクトリ構造の説明
