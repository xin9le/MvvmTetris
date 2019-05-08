using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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
                .Do(x =>
                {
                    //--- 一旦クリア
                    foreach (var c in this.Cells)
                        c.Color.Value = this.BackgroundColor;

                    //--- ブロック部分に色を塗る
                    var tetrimino = new Tetrimino(x, InitialPosition(x));
                    foreach (var b in tetrimino.Blocks)
                    {
                        var p = b.Position;
                        this.Cells[p.Row, p.Column].Color.Value = b.Color;
                    }
                })
                .Select(_ => Unit.Default)
                .ToReadOnlyReactivePropertySlim(mode: ReactivePropertyMode.None);  // subscribe and fire changed
        }
        #endregion


        #region 補助
        /// <summary>
        /// 次の場におけるブロックの初期位置を取得します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <returns>初期位置</returns>
        private static Position InitialPosition(TetriminoKind self)
        {
            var length = self.GetBoundingBoxLength();
            var row = (RowCount - length) / 2;
            var column = (ColumnCount - length) / 2;
            return new Position(row, column);
        }
        #endregion
    }
}
