# ボスシーンプロトタイプ 設計要約

## 概要

このドキュメントは、ボスシーンプロトタイプ（BossPrototype）の設計と実装について要約したものです。
本プロトタイプは、MVPアーキテクチャに基づいて設計され、Unity Timelineと状態機械パターンを組み合わせたフェーズ制御システムを実装しています。

## アーキテクチャ概要

### 設計原則

ボスシーンプロトタイプは、以下の設計原則に従って実装されています：

1. **MVPアーキテクチャの適用**: Model、View、Presenterの責務を明確に分離
2. **状態機械パターン**: フェーズ遷移をState Patternで管理
3. **Timeline駆動**: Unity Timelineを使用した弾幕パターンの制御
4. **UniRxによるリアクティブプログラミング**: イベント駆動とデータフローの管理
5. **オブジェクトプール**: 弾の生成・破棄を効率化

## 主要コンポーネント

### 1. ボス本体の制御

#### BossPresenter
- **責務**: ボスのライフサイクル管理とコンポーネント間の連携
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/Presenter/BossPresenter.cs`
- **主要機能**:
  - BossHealthModelの初期化と管理
  - BossPhaseStateMachineとの連携
  - HP変化の監視とログ出力
  - フェーズ変更イベントのハンドリング

#### BossView
- **責務**: ボスの視覚的表現と移動アニメーション
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/View/BossView.cs`
- **主要機能**:
  - フェーズごとの移動ポイント管理
  - DOTweenを使用した移動アニメーション
  - 各フェーズの移動回数カウント

#### BossHealthModel
- **責務**: ボスのHP管理とダメージ処理
- **配置場所**: `Assets/Project/Commons/BossPrototype/Scripts/Model/BossHealthModel.cs`
- **主要機能**:
  - 最大HPと現在HPの管理
  - ReactivePropertyによるHP変化の通知
  - ダメージ処理と死亡判定
  - IDisposableの実装によるリソース管理

### 2. フェーズ制御システム

#### BossPhaseStateMachine
- **責務**: フェーズ状態の管理と遷移制御
- **配置場所**: `Assets/Project/Commons/BossPrototype/Scripts/Presenter/BossPhaseStateMachine.cs`
- **主要機能**:
  - 現在のフェーズ状態の保持
  - フェーズ間の遷移処理（Enter/Exit）
  - 毎フレームの状態更新
  - HealthModelの参照管理

#### BossPhaseStateBase
- **責務**: フェーズ状態の基底クラス
- **配置場所**: `Assets/Project/Commons/BossPrototype/Scripts/Presenter/BossPhaseStateBase.cs`
- **設計パターン**: State Pattern
- **主要機能**:
  - Enter()、Update()、Exit()の抽象化
  - StateMachineとHealthModelへの参照

#### SimplePhaseState
- **責務**: 具体的なフェーズの実装
- **配置場所**: `Assets/Project/Commons/BossPrototype/Scripts/Presenter/SimplePhaseState.cs`
- **主要機能**:
  - Unity Timelineの制御（再生・停止）
  - HP閾値による遷移判定
  - 時間閾値による遷移判定
  - UniRxのObservable.Mergeによる複数条件の監視
  - 次のフェーズへの自動遷移

#### BossPrototypePhaseController
- **責務**: 各フェーズの初期化と連鎖設定
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/Presenter/BossPrototypePhaseController.cs`
- **主要機能**:
  - Phase1、Phase2、Phase3のTimelineの参照管理
  - フェーズの連鎖構造の構築
  - 初期フェーズの開始

### 3. 弾幕システム

#### BulletManagerBase
- **責務**: 弾の生成・管理の基底クラス
- **配置場所**: `Assets/Project/Commons/EnemyBulletPrototype/Scripts/Presenter/BulletManagerBase.cs`
- **主要機能**:
  - ObjectPoolによる弾のプーリング
  - アクティブな弾のリスト管理
  - 弾の生成（SpawnBullet）と回収（ReturnBullet）
  - クリーンアップ処理

#### SimpleBulletManager
- **責務**: 具体的な弾の生成実装
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/Presenter/SimpleBulletManager.cs`
- **主要機能**:
  - プールから弾のViewを取得
  - BulletModelの生成と初期化
  - EnemyBulletPresenterの生成と初期化
  - アクティブリストへの追加

#### EnemyBulletPresenter
- **責務**: 個々の弾のライフサイクル管理
- **配置場所**: `Assets/Project/Commons/EnemyBulletPrototype/Scripts/Presenter/EnemyBulletPresenter.cs`
- **主要機能**:
  - BulletModelとEnemyBulletViewBaseの連携
  - 衝突判定のイベントハンドリング
  - プレイヤーとの衝突処理
  - 寿命管理と自動破棄
  - プールへの返却処理

#### BulletModel
- **責務**: 弾のデータ管理
- **配置場所**: `Assets/Project/Commons/EnemyBulletPrototype/Scripts/Model/BulletModel.cs`
- **主要機能**:
  - ダメージ、HP、速度、寿命の管理
  - ダメージ処理と破壊判定

#### EnemyBulletViewBase
- **責務**: 弾の視覚的表現と物理挙動の基底クラス
- **配置場所**: `Assets/Project/Commons/EnemyBulletPrototype/Scripts/View/EnemyBulletViewBase.cs`
- **主要機能**:
  - 速度ベクトルによる移動処理
  - 寿命のカウントダウン
  - 衝突判定のSubject通知
  - ポーズ対応（Time.timeScale対応）

#### SimpleBulletView
- **責務**: シンプルな直線移動の弾の実装
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/View/SimpleBulletView.cs`
- **主要機能**:
  - 直線移動の実装（UpdateMovementのオーバーライド）

### 4. Timeline連携

#### BulletSpawnSignalAsset
- **責務**: Timelineからの弾生成シグナルデータ
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/Model/BulletSpawnSignalAsset.cs`
- **主要機能**:
  - 弾の位置、方向、速度のパラメータ保持
  - Unity SignalAssetの拡張

#### BulletSpawnSignalReceiver
- **責務**: Timelineシグナルの受信と処理
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/Presenter/BulletSpawnSignalReceiver.cs`
- **主要機能**:
  - Unity SignalReceiverの実装
  - シグナル受信時のBulletManagerBase.SpawnBullet呼び出し
  - シグナルアセットからのパラメータ抽出

### 5. デバッグ機能

#### DebugController
- **責務**: デバッグ用のテスト機能
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/Presenter/DebugController.cs`
- **主要機能**:
  - キーボード入力によるダメージテスト（Space: 10%, D: 30%, K: 即死）
  - HP表示機能（Hキー）
  - ポーズ機能（Escキー）
  - BackgroundViewとの連携

#### BackgroundView
- **責務**: 背景の視覚表現とポーズ時の色変更
- **配置場所**: `Assets/Project/Scenes/BossPrototype/Scripts/View/BackgroundView.cs`
- **主要機能**:
  - ポーズ時の背景色変更（グレー）
  - 再開時の背景色復元（白）

## データフロー

### フェーズ遷移フロー

1. **初期化フェーズ**:
   ```
   BossPresenter.Awake()
   → BossHealthModel生成
   → BossPhaseStateMachine.Init()
   → BossPrototypePhaseController.Start()
   → SimplePhaseState生成（Phase3 → Phase2 → Phase1の順で連鎖）
   → Phase1に遷移
   ```

2. **フェーズ実行中**:
   ```
   SimplePhaseState.Enter()
   → Timeline再生開始
   → HP/時間閾値の監視開始（Observable.Merge）
   → Timeline内でBulletSpawnSignalが発火
   → BulletSpawnSignalReceiver.OnBulletSpawn()
   → BulletManagerBase.SpawnBullet()
   ```

3. **フェーズ遷移トリガー**:
   ```
   HP閾値到達 または 時間閾値到達
   → SimplePhaseState.TriggerTransition()
   → BossPhaseStateMachine.TransitionTo()
   → 現在のState.Exit() → Timeline停止
   → 次のState.Enter() → 次のTimeline開始
   ```

4. **ボス撃破時**:
   ```
   最終フェーズでnextState == null
   → Timeline停止
   → "Boss Defeated!"ログ出力
   ```

### 弾の生成から破棄までのフロー

1. **弾の生成**:
   ```
   Timeline Signal発火
   → BulletSpawnSignalReceiver
   → SimpleBulletManager.SpawnBullet()
   → ObjectPoolからView取得
   → BulletModel生成
   → EnemyBulletPresenter生成・初期化
   → View初期化（速度・寿命設定）
   → activeBulletsリストに追加
   ```

2. **弾の更新**:
   ```
   毎フレーム
   → EnemyBulletViewBase.Update()
   → UpdateMovement()（移動処理）
   → 寿命カウントダウン
   → 寿命切れ時: OnLifeTimeExpiredSubject発火
   ```

3. **衝突検出**:
   ```
   OnTriggerEnter2D()
   → OnHitSubject.OnNext(Collider2D)
   → EnemyBulletPresenter.HandleHit()
   → プレイヤータグ判定
   → ダメージ処理
   → DestroyBullet()呼び出し
   ```

4. **弾の破棄**:
   ```
   EnemyBulletPresenter.DestroyBullet()
   → Dispose()（購読解除）
   → View.Cleanup()
   → BulletManagerBase.ReturnBullet()
   → ObjectPoolに返却
   → activeBulletsリストから削除
   ```

## 使用技術とライブラリ

### 主要ライブラリ

1. **UniRx**: リアクティブプログラミング
   - ReactiveProperty: HP変化の監視
   - Subject: イベント通知（衝突、寿命切れ）
   - Observable.Merge: 複数条件の統合（HP閾値と時間閾値）
   - Observable.Timer: 時間ベースのフェーズ遷移

2. **DOTween**: アニメーション
   - ボスの移動アニメーション（DOMove）

3. **Unity Timeline**: 弾幕パターンの制御
   - PlayableDirector: タイムライン再生制御
   - SignalAsset/SignalReceiver: タイムラインイベントの受信

4. **ObjectPool**: メモリ管理
   - UnityEngine.Pool.ObjectPool: 弾のプーリング

## 設計の特徴と利点

### 1. 拡張性の高い設計

- **State Pattern**: 新しいフェーズタイプを追加しやすい
  - BossPhaseStateBaseを継承して新しい振る舞いを実装可能
  - 既存コードへの影響を最小化

- **抽象化による柔軟性**:
  - BulletManagerBaseを継承して異なる弾幕パターンを実装可能
  - EnemyBulletViewBaseを継承して異なる移動パターンを実装可能

### 2. Timeline駆動による弾幕制御

- **デザイナーフレンドリー**:
  - 弾幕パターンをTimelineエディタで視覚的に編集可能
  - プログラマーの介入なしでパターン調整が可能

- **シグナルシステム**:
  - BulletSpawnSignalAssetで弾のパラメータを定義
  - タイムラインから弾生成をトリガー

### 3. リアクティブなデータフロー

- **UniRxによるイベント駆動**:
  - HP変化を購読して自動的にUI更新
  - 複数の遷移条件（HP/時間）を統合的に監視
  - Subject/Observableによる疎結合な設計

### 4. パフォーマンス最適化

- **ObjectPoolパターン**:
  - 弾の頻繁な生成・破棄によるGC圧を軽減
  - メモリアロケーションの削減

- **効率的な状態管理**:
  - 非アクティブなTimelineの自動停止
  - 使用中の弾のみをリストで管理

### 5. デバッグ機能の充実

- **DebugController**:
  - 開発中のテストを効率化
  - フェーズ遷移のテストが容易
  - ポーズ機能による詳細確認

## 改善可能な点

以下は、今後の実装で検討すべき改善点です：

1. **プレイヤーとの統合**:
   - プレイヤーへのダメージ処理が未実装（TODO コメントあり）
   - プレイヤーの攻撃を受ける処理が部分的に実装

2. **エフェクトシステム**:
   - 弾の破壊エフェクトが未実装（TODO コメントあり）
   - ボスのダメージエフェクトなし

3. **UI表示**:
   - HPバーなどのUI要素が未実装
   - 現在はログ出力のみ

4. **サウンド**:
   - BGMや効果音の統合が未実装

5. **リソース管理**:
   - アクティブな弾のクリーンアップが手動（Cleanup()呼び出しが必要）

## まとめ

ボスシーンプロトタイプは、MVPアーキテクチャと状態機械パターンを基盤とし、Unity Timelineを活用した柔軟な弾幕制御システムを実現しています。

**主要な特徴**:
- 明確な責務分離とモジュール化された設計
- Timeline駆動による視覚的な弾幕パターン編集
- UniRxによるリアクティブなイベント処理
- ObjectPoolによるパフォーマンス最適化
- 拡張性の高い状態管理システム

このプロトタイプは、今後のボスバトル実装の基盤として十分に機能する設計となっており、新しいボスや弾幕パターンの追加が容易に行えます。
