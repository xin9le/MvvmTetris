using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MvvmTetris.Collections.Generic;
using MvvmTetris.Engine.Models;
using MvvmTetris.Linq;
using Reactive.Bindings;



namespace MvvmTetris.Engine.ViewModels
{
    /// <summary>
    /// フィールド描画用のモデルを提供します。
    /// </summary>
    public class FieldViewModel : IFieldViewModel
    {
        #region イベント
        /// <summary>
        /// 変更されたときに呼び出されます。
        /// </summary>
        public IObservable<Unit> Changed => this.changed;
        private readonly Subject<Unit> changed = new Subject<Unit>();
        #endregion


        #region プロパティ
        /// <summary>
        /// テトリスの場を取得します。
        /// </summary>
        private Field Field { get; }


        /// <summary>
        /// セルのコレクションを取得します。
        /// </summary>
        public CellViewModel[,] Cells { get; }


        /// <summary>
        /// アクティブ状態かどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsActivated => this.Field.IsActivated;


        /// <summary>
        /// 背景色を取得します。
        /// </summary>
        private Color BackgroundColor => Color.WhiteSmoke;
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        internal FieldViewModel(Field field)
        {
            this.Field = field;

            //--- 描画するセルを準備
            this.Cells = new CellViewModel[Field.RowCount, Field.ColumnCount];
            foreach (var item in this.Cells.WithIndex())
                this.Cells[item.X, item.Y] = new CellViewModel();

            //--- 新規生成されたテトリミノを保存しておく
            Tetrimino previous = null;
            this.Field.Tetrimino
                .Where(x => x != null)
                .Subscribe(x => previous = x.Clone());

            //--- 確定したら前回値を削除
            this.Field.TetriminoFixed.Subscribe(_ => previous = null);

            //--- 移動 / 回転したらセルを更新
            this.Field.TetriminoMoved
                .Merge(this.Field.TetriminoRotated)
                .Subscribe(_ =>
                {
                    //--- 前回位置のセルを背景色に戻す
                    if (previous != null)
                    {
                        foreach (var x in previous.Blocks)
                        {
                            var p = x.Position;
                            if (p.Row < 0) continue;
                            if (p.Column < 0) continue;
                            this.Cells[p.Row, p.Column].Color.Value = this.BackgroundColor;
                        }
                        previous = null;
                    }

                    //--- 今回位置のセルにブロック色を適用
                    var current = this.Field.Tetrimino.Value;
                    if (current != null)
                    {
                        foreach (var x in current.Blocks)
                        {
                            var p = x.Position;
                            if (p.Row < 0) continue;
                            if (p.Column < 0) continue;
                            this.Cells[p.Row, p.Column].Color.Value = x.Color;
                        }

                        //--- 前回値保持
                        previous = current.Clone();
                        this.changed.OnNext(Unit.Default);
                    }
                });

            //--- ブロックが消されたら配置済みブロックで塗り直す
            this.Field.BlockRemoved.Subscribe(_ =>
            {
                var blocks
                    = this.Field.PlacedBlocks
                    .Where(x => x.Position.Row >= 0)  // 見切れている分は無視
                    .ToDictionary2
                    (
                        x => x.Position.Row,
                        x => x.Position.Column,
                        x => (Block?)x
                    );
                foreach (var item in this.Cells.WithIndex())
                {
                    var color = blocks.GetValueOrDefault(item.X)
                                ?.GetValueOrDefault(item.Y)
                                ?.Color
                                ?? this.BackgroundColor;
                    item.Element.Color.Value = color;
                }
                this.changed.OnNext(Unit.Default);
            });

            //--- すべてのセルを背景色に戻す
            this.Field.BlockCleared.Subscribe(_ =>
            {
                for (int r = 0; r < this.Cells.GetLength(0); r++)
                    for (int c = 0; c < this.Cells.GetLength(1); c++)
                        this.Cells[r, c].Color.Value = this.BackgroundColor;

                this.changed.OnNext(Unit.Default);
            });
        }
        #endregion
    }
}
