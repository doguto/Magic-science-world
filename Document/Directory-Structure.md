# ディレクトリ構造

このドキュメントは、Magic-science-worldプロジェクトのディレクトリ構造について説明します。

## プロジェクト概要

- **Unity バージョン**: 6000.0.23f1
- **アーキテクチャ**: MVP (Model-View-Presenter)
- **使用技術**: UniRx, UniTask, DOTween

## ルートディレクトリ

```
Magic-science-world/
├── Assets/                    # Unity アセットファイル
├── Document/                  # プロジェクトドキュメント
├── Packages/                  # Unity Package Manager設定
├── ProjectSettings/           # Unity プロジェクト設定
├── .github/                   # GitHub関連設定
├── .gitignore                # Git除外設定
└── README.md                 # プロジェクト概要
```

## Assetsディレクトリ構造

### 主要ディレクトリ

```
Assets/
├── AddressableAssetsData/     # Addressableアセット管理
├── Editor/                    # エディター拡張
├── Plugins/                   # サードパーティプラグイン
├── Project/                   # メインプロジェクトアセット
├── Resources/                 # Unityリソース
├── Settings/                  # プロジェクト設定
└── TextMesh Pro/              # TextMeshPro関連アセット
```

### Project ディレクトリ詳細

プロジェクトの核となるアセットが格納されています：

```
Assets/Project/
├── AssemblyDifinitions/       # アセンブリ定義
│   └── Constants/             # 定数定義アセンブリ
├── Commons/                   # 共通アセット
│   ├── Prefabs/              # 共通プレハブ
│   │   └── Player/           # プレイヤー関連プレハブ
│   └── Scripts/              # 共通スクリプト
│       ├── Constants/        # 定数クラス
│       ├── Extensions/       # 拡張メソッド
│       ├── Model/           # 共通モデル
│       ├── Presenter/       # 共通プレゼンター
│       └── View/            # 共通ビュー
│           └── UI/          # UI関連ビュー
├── DataStore/                 # データ保存領域
├── DeprecatedResources/       # 旧リソース（非推奨）
├── Samples/                   # サンプルコード
├── Scenes/                    # シーン関連アセット
├── Scripts/                   # グローバルスクリプト
└── Textures/                  # テクスチャアセット
```

## シーン構造

各シーンはMVPアーキテクチャに従って構成されています：

```
Assets/Project/Scenes/
├── BattleBoss/                # ボス戦シーン
│   └── Scripts/
│       ├── Model/             # ボス戦データモデル
│       ├── Presenter/         # ボス戦ロジック
│       └── View/              # ボス戦UI
├── BattleWay/                 # 通常戦闘シーン
│   └── Scripts/
│       ├── Model/             # 戦闘データモデル
│       ├── Presenter/         # 戦闘ロジック
│       └── View/              # 戦闘UI
├── Global/                    # グローバル管理シーン
│   └── Scripts/
│       ├── Model/             # グローバルデータ
│       ├── Presenter/         # グローバル制御
│       └── View/              # グローバルUI
├── QuestList/                 # クエスト一覧シーン
│   ├── Prefabs/               # クエスト関連プレハブ
│   └── Scripts/
│       ├── Model/             # クエストデータモデル
│       ├── Presenter/         # クエスト管理ロジック
│       └── View/              # クエストUI
├── Scenario/                  # シナリオシーン
│   └── Scripts/
│       ├── Model/             # シナリオデータモデル
│       ├── Presenter/         # シナリオ進行制御
│       └── View/              # シナリオUI
└── Title/                     # タイトルシーン
    └── Scripts/
        ├── Model/             # タイトルデータモデル
        ├── Presenter/         # タイトル制御
        └── View/              # タイトルUI
```

## アセンブリ定義構造

プロジェクトはモジュラー設計されており、以下のアセンブリに分割されています：

### プロジェクトアセンブリ

```
Assets/Project/Scripts/
├── Extensions/                # Extensions.asmdef
├── Infra/                     # Infra.asmdef
├── Model/                     # Model.asmdef
├── Presenter/                 # Presenter.asmdef
├── Repository/                # Repository.asmdef
└── View/                      # View.asmdef
```

### 共通アセンブリ

```
Assets/Project/AssemblyDifinitions/
└── Constants/                 # Constants.asmdef
```

### Editor ディレクトリ

```
Assets/Editor/
└── Novel/                     # ノベルゲーム用エディター拡張
```

### Settings ディレクトリ

```
Assets/Settings/
├── Build Profiles/            # ビルドプロファイル
├── Scenes/                    # シーン設定
├── Renderer2D.asset          # 2Dレンダラー設定
└── UniversalRP.asset         # Universal Render Pipeline設定
```

### Packages

プロジェクトはPackage Managerを使用して依存関係を管理しています：

- `Packages/manifest.json` - パッケージ依存関係定義
- `Packages/packages-lock.json` - パッケージバージョンロック

```
Assets/Plugins/
├── Demigiant/                 # DOTween プラグイン
│   └── DOTween/
│       └── Modules/           # DOTweenモジュール
├── UniRx/                     # UniRx プラグイン
│   ├── Examples/              # サンプルコード
│   └── Scripts/               # UniRx コアスクリプト
└── UniTask/                   # UniTask プラグイン
    ├── Editor/                # エディター拡張
    └── Runtime/               # ランタイムライブラリ
        ├── External/          # 外部連携
        └── Linq/              # LINQ拡張
```

## リソース構造

```
Assets/Project/Textures/
├── Character/                 # キャラクター画像
│   ├── Azuma/                # アズマ
│   ├── Hanare/               # ハナレ
│   ├── Kon/                  # コン
│   ├── Loco/                 # ロコ
│   ├── Sui/                  # スイ
│   ├── Tatsumi/              # タツミ
│   ├── Ten/                  # テン（主人公）
│   └── Ushitora/             # ウシトラ
│       ├── Dot/              # ドット絵
│       ├── Face/             # 表情画像
│       └── Still/            # スチル画像
├── Images/                    # 一般画像
│   └── BackGround/           # 背景画像
├── Scenario/                  # シナリオ関連画像
└── Sounds/                    # 音声ファイル
```

### キャラクターリソース

プロジェクトには8体のキャラクターが定義されており、それぞれに以下のリソースが含まれています：

- **主人公**: テン
- **その他キャラクター**: アズマ、ハナレ、コン、ロコ、スイ、タツミ、ウシトラ

各キャラクターディレクトリには：
- `Dot/`: ドット絵スプライト
- `Face/`: 表情画像（シナリオ用）
- `Still/`: スチル画像

## 命名規則

### ネームスペース

```csharp
// シーン固有
Project.Scenes.[シーン名].Scripts.[Model|View|Presenter]

// 共通機能
Project.Commons.Scripts.[機能名]

// グローバル機能
Project.Scripts.[機能名]
```

### ファイル命名

- **Model**: `[機能名]Model.cs`
- **View**: `[機能名]View.cs`
- **Presenter**: `[機能名]Presenter.cs`

## 設計原則

1. **MVPアーキテクチャの厳格な遵守**
   - Model: データとロジック (MonoBehaviour非継承)
   - View: UI表示制御 (MonoBehaviour継承)
   - Presenter: ModelとViewの仲介 (MonoBehaviour継承)

2. **明確な責任分離**
   - 各層は明確に分離され、依存関係は一方向

3. **アセンブリによるモジュール化**
   - 機能別にアセンブリを分割し、適切な依存関係を維持

4. **リソースの組織化**
   - 機能別・用途別にリソースを分類して配置