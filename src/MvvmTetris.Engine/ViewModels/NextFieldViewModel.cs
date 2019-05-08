using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using MvvmTetris.Collections.Generic;
using MvvmTetris.Engine.Models;
using MvvmTetris.Linq;
using Reactive.Bindings;



namespace MvvmTetris.Engine.ViewModels
{
    /// <summary>
    /// 次のテトリミノを表示するためのフィールド用のモデルを提供します。
    /// </summary>
    public class NextFieldViewModel : IFieldViewModel
    {
        #region 定数
        /// <summary>
        /// 行数を表します。この値は定数です。
        /// </summary>
        private const byte RowCount = 5;


        /// <summary>
        /// 列数を表します。この値は定数です。
        /// </summary>
        private const byte ColumnCount = 5;
        #endregion


        #region イベント
        /// <summary>
        /// 変更されたときに呼び出されます。
        /// </summary>
        public IObservable<Unit> Changed { get; }
        #endregion


        #region プロパティ
        /// <summary>
        /// セルのコレクションを取得します。
        /// </summary>
        public CellViewModel[,] Cells { get; }


        /// <summary>
        /// 背景色を取得します。
        /// </summary>
        private Color BackgroundColor => Color.WhiteSmoke;
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="nextTetrimino">次のテトリミノ</param>
        public NextFieldViewModel(IReadOnlyReactiveProperty<TetriminoKind> nextTetrimino)
        {
            //--- 描画するセルを準備
            this.Cells = new CellViewModel[RowCount, ColumnCount];
            foreach (var item in this.Cells.WithIndex())
                this.Cells[item.X, item.Y] = new CellViewModel();

            //--- ブロックに関する変更を処理
            this.Changed
                = nextTetrimino
                .Select(x => Tetrimino.Create(x).Blocks.ToDictionary2(y => y.Position.Row, y => y.Position.Column))
                .Do(x =>
                {
                    //--- ViewPort のオフセット調整
                    //--- ちゃんと書くのがだいぶ面倒臭くなったから無理やりやる
                    var offset = new Position((-6 - x.Count) / 2, 2);

                    //--- 適用
                    foreach (var item in this.Cells.WithIndex())
                    {
                        var color = x.GetValueOrDefault(item.X + offset.Row)
                                    ?.GetValueOrDefault(item.Y + offset.Column)
                                    ?.Color
                                    ?? this.BackgroundColor;
                        item.Element.Color.Value = color;
                    }
                })
                .Select(_ => Unit.Default)
                .ToReadOnlyReactivePropertySlim(mode: ReactivePropertyMode.None);  // subscribe and fire changed
        }
        #endregion
    }
}
