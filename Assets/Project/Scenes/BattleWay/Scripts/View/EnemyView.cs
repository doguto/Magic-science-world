// Assets/Project/Scenes/BattleWay/Scripts/View/EnemyView.cs
using UnityEngine;

namespace Project.Scenes.BattleWay.Scripts.View
{
    using Project.Commons.DataBase.Scripts;
    using Project.Scenes.BattleWay.Scripts.View.Pool;
    using Project.Scenes.BattleWay.Scripts.View.Movement;

    /// <summary>
    /// 敵のView層コンポーネント。
    /// Presenterから移動パターンとシューター設定を受け取り、
    /// MoveExecutorとEnemyShooterに処理を委譲する。
    /// </summary>
    public class EnemyView : MonoBehaviour
    {
        [Header("デバッグ用（本来はPresenterから設定される）")]
        [SerializeField] private EnemyMovePatternSO _debugMovePattern;

        [Header("シューター設定")]
        [SerializeField] private EnemyShooter _shooter;
        [SerializeField, Tooltip("射撃位置の上書き（未指定ならShooter側設定を使用）")]
        private Transform _firePointOverride;

        private MoveExecutor _moveExecutor;
        private EnemyMovePatternSO _currentMovePattern;
        private bool _isInitializedByPresenter;

        /// <summary>
        /// Presenterから移動パターンで初期化
        /// </summary>
        public void Initialize(EnemyMovePatternSO movePattern)
        {
            _currentMovePattern = movePattern;
            _isInitializedByPresenter = true;
            
            // MoveExecutorの初期化
            _moveExecutor ??= new MoveExecutor(transform);
        }

        /// <summary>
        /// 位置指定付きで初期化
        /// </summary>
        public void Initialize(EnemyMovePatternSO movePattern, Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            Initialize(movePattern);
        }

        /// <summary>
        /// 移動開始（現在設定されているパターンで実行）
        /// </summary>
        public void StartMovement()
        {
            var pattern = GetActivePattern();
            if (pattern == null)
            {
                Debug.LogWarning("[EnemyView] 移動パターンが設定されていません。");
                return;
            }

            _moveExecutor ??= new MoveExecutor(transform);
            _moveExecutor.ExecuteMove(pattern);
        }

        /// <summary>
        /// 移動停止
        /// </summary>
        public void StopMovement()
        {
            _moveExecutor?.StopCurrentMove();
        }

        /// <summary>
        /// シューターを設定・起動
        /// </summary>
        public void ConfigureShooter(BulletPatternSO bulletPattern, BulletPool bulletPool, Transform customFirePoint = null)
        {
            if (bulletPattern == null || bulletPool == null)
            {
                Debug.LogWarning("[EnemyView] シューター設定に必要なパラメータが不足しています。");
                return;
            }

            // Shooterコンポーネントの取得
            if (_shooter == null)
            {
                _shooter = GetComponentInChildren<EnemyShooter>(true);
            }

            if (_shooter == null)
            {
                Debug.LogWarning("[EnemyView] EnemyShooterコンポーネントが見つかりません。");
                return;
            }

            // 射撃位置の決定（優先度: カスタム > 設定済み > Shooter側デフォルト）
            var firePoint = customFirePoint ?? _firePointOverride;
            _shooter.Setup(bulletPattern, bulletPool, firePoint);
        }

        private void Start()
        {
            // デバッグ用自動実行（Presenterから初期化されていない場合のみ）
            if (!_isInitializedByPresenter && _debugMovePattern != null)
            {
                Initialize(_debugMovePattern);
                StartMovement();
            }
        }

        private void OnDestroy()
        {
            _moveExecutor?.Dispose();
        }

        /// <summary>
        /// 有効な移動パターンを取得（Presenter設定優先、fallbackでデバッグ用）
        /// </summary>
        private EnemyMovePatternSO GetActivePattern()
        {
            return _currentMovePattern ?? _debugMovePattern;
        }
    }
}
