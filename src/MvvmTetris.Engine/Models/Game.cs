using System;
using System.Reactive.Linq;
using Reactive.Bindings;



namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// ゲーム本体を表します。
    /// </summary>
    public class Game
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


        /// <summary>
        /// 前回のスピードアップ回数を取得または設定します。
        /// </summary>
        private int PreviousCount { get; set; }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public Game()
        {
            this.Field.PlacedBlocks.Subscribe(_ =>
            {
                //--- 10 行消すたびにスピードアップ
                var count = this.Score.TotalRowCount.Value / 10;
                if (count > this.PreviousCount)
                {
                    this.PreviousCount = count;
                    this.Field.SpeedUp();
                }

                //--- 新しいテトリミノを設定
                var kind = this.nextTetrimino.Value;
                this.nextTetrimino.Value = Tetrimino.RandomKind();
                this.Field.Tetrimino.Value = new Tetrimino(kind, kind.InitialFieldPosition());
            });
            this.Field.LastRemovedRowCount.Subscribe(this.Score.AddRowCount);
        }
        #endregion


        #region 操作
        /// <summary>
        /// ゲームを開始します。
        /// </summary>
        public void Play()
        {
            this.Field.Reset();
            this.PreviousCount = 0;
            this.nextTetrimino.Value = Tetrimino.RandomKind();
            this.Field.Start(Tetrimino.RandomKind());
            this.Score.Clear();
        }
        #endregion
    }
}
