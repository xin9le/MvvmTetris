using System.Drawing;



namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// ブロックとしての機能を提供します。
    /// </summary>
    public readonly struct Block
    {
        #region プロパティ
        /// <summary>
        /// 色を取得します。
        /// </summary>
        public Color Color { get; }


        /// <summary>
        /// 座標を取得します。
        /// </summary>
        public Position Position { get; }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="color">ブロックの色</param>
        /// <param name="position">ブロックの位置</param>
        public Block(Color color, Position position)
        {
            this.Color = color;
            this.Position = position;
        }
        #endregion
    }
}
