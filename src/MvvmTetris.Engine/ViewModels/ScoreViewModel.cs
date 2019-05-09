using Reactive.Bindings;
using MvvmTetris.Engine.Models;



namespace MvvmTetris.Engine.ViewModels
{
    /// <summary>
    /// ゲームスコア結果を表します。
    /// </summary>
    public class ScoreViewModel
    {
        #region プロパティ
        /// <summary>
        /// ゲームスコアを取得します。
        /// </summary>
        private Score Score { get; }


        /// <summary>
        /// 消した合計行数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> TotalRowCount => this.Score.TotalRowCount;


        /// <summary>
        /// 1 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount1 => this.Score.RowCount1;


        /// <summary>
        /// 2 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount2 => this.Score.RowCount2;


        /// <summary>
        /// 3 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount3 => this.Score.RowCount3;


        /// <summary>
        /// 4 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount4 => this.Score.RowCount4;
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="score">ゲームスコア</param>
        internal ScoreViewModel(Score score)
            => this.Score = score;
        #endregion
    }
}
