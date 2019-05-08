using System;
using System.Drawing;
using System.Linq;



namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// テトリミノの種類を表します。
    /// </summary>
    public enum TetriminoKind
    {
        /// <summary>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// </summary>
        I = 0,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>□■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        O,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>■■□□</para>
        /// <para>□□□□</para>
        /// </summary>
        S,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>□□■■</para>
        /// <para>□□□□</para>
        /// </summary>
        Z,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■□□</para>
        /// <para>□■■■</para>
        /// <para>□□□□</para>
        /// </summary>
        J,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□□■□</para>
        /// <para>■■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        L,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■□□</para>
        /// <para>■■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        T,
    }



    /// <summary>
    /// テトリミノの種類の拡張機能を提供します。
    /// </summary>
    internal static class TetriminoExtensions
    {
        /// <summary>
        /// ブロックの色を取得します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <returns>色</returns>
        public static Color BlockColor(this TetriminoKind self)
        {
            switch (self)
            {
                case TetriminoKind.I:   return Color.LightBlue;
                case TetriminoKind.O:   return Color.Yellow;
                case TetriminoKind.S:   return Color.YellowGreen;
                case TetriminoKind.Z:   return Color.Red;
                case TetriminoKind.J:   return Color.Blue;
                case TetriminoKind.L:   return Color.Orange;
                case TetriminoKind.T:   return Color.Purple;
            }
            throw new InvalidOperationException("Unknown Tetrimino");
        }


        /// <summary>
        /// 場におけるブロックの初期位置を取得します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <returns>初期位置</returns>
        public static Position InitialPosition(this TetriminoKind self)
        {
            var length = self.GetBoundingBoxLength();
            var row = -length;
            var column = (Field.ColumnCount - length) / 2;
            return new Position(row, column);
        }


        /// <summary>
        /// ブロックを囲う最小矩形 (正方形) の一辺の長さを取得します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <returns>長さ</returns>
        public static int GetBoundingBoxLength(this TetriminoKind self)
        {
            switch (self)
            {
                case TetriminoKind.I: return 4;
                case TetriminoKind.O: return 2;
                case TetriminoKind.S:
                case TetriminoKind.Z:
                case TetriminoKind.J:
                case TetriminoKind.L:
                case TetriminoKind.T: return 3;
                default: throw new InvalidOperationException("Unknown Tetrimino");
            }
        }


        /// <summary>
        /// 指定されたテトリミノの種類と方向に一致したブロックを生成します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <param name="offset">絶対座標への移動分</param>
        /// <param name="direction">方向</param>
        /// <returns>ブロックのコレクション</returns>
        public static Block[] CreateBlock(this TetriminoKind self, Position offset, Direction direction = Direction.Up)
        {
            //--- ブロック形状をビットで表現
            //--- ベタ書きだけど、これが最も分かりやすく高速
            int[,] pattern = null;
            switch (self)
            {
                #region I
                case TetriminoKind.I:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region O
                case TetriminoKind.O:
                    pattern = new int[,]
                    {
                        { 1, 1 },
                        { 1, 1 },
                    };
                    break;
                #endregion

                #region S
                case TetriminoKind.S:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region Z
                case TetriminoKind.Z:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 1, 0, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region J
                case TetriminoKind.J:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region L
                case TetriminoKind.L:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 1, 0, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region T
                case TetriminoKind.T:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion
            }

            //--- どれにも当てはまらなかった
            if (pattern == null)
                throw new InvalidOperationException("Unknown Tetrimino");

            //--- ビットが立っている部分にブロックを作成
            var color = self.BlockColor();
            return  Enumerable.Range(0, pattern.GetLength(0))
                    .SelectMany(r => Enumerable.Range(0, pattern.GetLength(1)).Select(c => new Position(r, c)))
                    .Where(x => pattern[x.Row, x.Column] != 0)  //--- bit が立っているところ
                    .Select(x => new Position(x.Row + offset.Row, x.Column + offset.Column))  //--- 絶対座標変換
                    .Select(x => new Block(color, x))
                    .ToArray();
        }
    }
}
