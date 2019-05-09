using System;
using System.Reactive.Linq;
using Reactive.Bindings;



namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// ゲーム本体を表します。
    /// </summary>
    internal class Game
    {
        #region プロパティ
        /// <summary>
        /// ゲームスコアを取得します。
        /// </summary>
        public Score Score { get; } = new Score();


        /// <summary>
        /// フィールドを取得します。
        /// </summary>
        public Field Field { get; } = new Field();


        /// <summary>
        /// プレイ中かどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsPlaying => this.Field.IsActivated;


        /// <summary>
        /// ゲームオーバー状態になっているかどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsOver => this.Field.IsUpperLimitOvered;


        /// <summary>
        /// 次に出現するテトリミノを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<TetriminoKind> NextTetrimino => this.nextTetrimino;
        private readonly ReactivePropertySlim<TetriminoKind> nextTetrimino = new ReactivePropertySlim<TetriminoKind>();
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public Game()
        {
            var speedUpCounter = 0;
            this.Field.BlockCleared.Subscribe(_ => speedUpCounter = 0);
            this.Field.BlockRemoved.Subscribe(this.Score.AddRowCount);
            this.Field.TetriminoFixed.Subscribe(_ =>
            {
                //--- 新しいテトリミノを設定
                var kind = this.nextTetrimino.Value;
                this.nextTetrimino.Value = Tetrimino.RandomKind();
                this.Field.Tetrimino.Value = new Tetrimino(kind);
            });
            this.Score.TotalRowCount.Subscribe(x =>
            {
                //--- 10 行消すたびにスピードアップ
                var count = x / 10;
                if (count > speedUpCounter)
                {
                    speedUpCounter = count;
                    this.Field.SpeedUp();
                }
            });
        }
        #endregion


        #region 操作
        /// <summary>
        /// ゲームを開始します。
        /// </summary>
        public void Play()
        {
            this.Field.Reset();
            this.nextTetrimino.Value = Tetrimino.RandomKind();
            this.Field.Start(Tetrimino.RandomKind());
            this.Score.Clear();
        }
        #endregion
    }
}
