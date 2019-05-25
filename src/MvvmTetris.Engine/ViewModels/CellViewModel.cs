using System.Drawing;
using Reactive.Bindings;



namespace MvvmTetris.Engine.ViewModels
{
    /// <summary>
    /// セル描画用のモデルを提供します。
    /// </summary>
    public class CellViewModel
    {
        #region 定数
        /// <summary>
        /// 既定の色を表します。
        /// </summary>
        public static readonly Color DefaultColor = System.Drawing.Color.WhiteSmoke;
        #endregion


        #region プロパティ
        /// <summary>
        /// 色を取得します。
        /// </summary>
        public ReactivePropertySlim<Color> Color { get; } = new ReactivePropertySlim<Color>(DefaultColor);
        #endregion
    }
}
