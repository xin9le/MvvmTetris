﻿using System.Drawing;



namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// ブロックとしての機能を提供します。
    /// </summary>
    internal readonly struct Block
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
        public Block(in Color color, in Position position)
        {
            this.Color = color;
            this.Position = position;
        }
        #endregion
    }
}
